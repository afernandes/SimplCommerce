using System.Collections.Generic;
using SimplCommerce.Module.Currencies.Domain;

namespace SimplCommerce.Module.Currencies.Services
{
    public interface IExchangeRateProvider
    {
        // <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode);
    }
}
