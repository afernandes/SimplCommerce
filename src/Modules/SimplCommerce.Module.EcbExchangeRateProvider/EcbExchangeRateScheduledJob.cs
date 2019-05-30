using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimplCommerce.Module.Currencies.Services;
using SimplCommerce.Module.HangfireJobs.Models;

namespace SimplCommerce.Module.EcbExchangeRateProvider
{
    public class EcbExchangeRateScheduledJob : ScheduledJob
    {
        #region Fields

        //private readonly CurrencySettings _currencySettings;
        private readonly ICurrencyService _currencyService;

        #endregion

        #region Ctor

        public EcbExchangeRateScheduledJob(
            //CurrencySettings currencySettings,
            ICurrencyService currencyService)
        {
            //_currencySettings = currencySettings;
            _currencyService = currencyService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        protected override Task ExecuteAsync(Hangfire.IJobCancellationToken cancellationToken)
        {
            var exchangeRates = _currencyService.GetCurrencyLiveRates();
            foreach (var exchageRate in exchangeRates)
            {
                var currency = _currencyService.GetCurrencyByCode(exchageRate.CurrencyCode, false);
                if (currency == null)
                    continue;

                currency.Rate = exchageRate.Rate;
                currency.UpdatedOnUtc = DateTime.UtcNow;
                _currencyService.UpdateCurrency(currency);
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
