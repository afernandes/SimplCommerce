using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Core.Services;

namespace SimplCommerce.Module.EcbExchangeRateProvider
{
    public class EcbExchangeRateProvider : IExchangeRateProvider
    {
        #region Fields

        private readonly IStringLocalizer _localizer;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public EcbExchangeRateProvider(IStringLocalizer localizer,
            ILogger logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        #endregion

        public IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
        {
            if (exchangeRateCurrencyCode == null)
                throw new ArgumentNullException(nameof(exchangeRateCurrencyCode));

            //add euro with rate 1
            var ratesToEuro = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    CurrencyCode = "EUR",
                    Rate = 1,
                    UpdatedOn = DateTime.UtcNow
                }
            };

            //get exchange rates to euro from European Central Bank
            var request = (HttpWebRequest)WebRequest.Create("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
            try
            {
                using (var response = request.GetResponse())
                {
                    //load XML document
                    var document = new XmlDocument();
                    document.Load(response.GetResponseStream());

                    //add namespaces
                    var namespaces = new XmlNamespaceManager(document.NameTable);
                    namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                    namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                    //get daily rates
                    var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                    if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime updateDate))
                        updateDate = DateTime.UtcNow;

                    foreach (XmlNode currency in dailyRates.ChildNodes)
                    {
                        //get rate
                        if (!decimal.TryParse(currency.Attributes["rate"].Value, out decimal currencyRate))
                            continue;

                        ratesToEuro.Add(new ExchangeRate()
                        {
                            CurrencyCode = currency.Attributes["currency"].Value,
                            Rate = currencyRate,
                            UpdatedOn = updateDate
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                _logger.LogError("ECB exchange rate provider", ex);
            }

            //return result for the euro
            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
                return ratesToEuro;

            //use only currencies that are supported by ECB
            var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new Exception(_localizer["Plugins.ExchangeRate.EcbExchange.Error"]);

            //return result for the selected (not euro) currency
            return ratesToEuro.Select(rate => new ExchangeRate
            {
                CurrencyCode = rate.CurrencyCode,
                Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
                UpdatedOn = rate.UpdatedOn
            }).ToList();
        }
    }
}
