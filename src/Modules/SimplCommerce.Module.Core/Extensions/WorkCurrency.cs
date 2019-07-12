using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Core.Services;

namespace SimplCommerce.Module.Core.Extensions
{
    public class WorkCurrency : IWorkCurrency
    {                
        private readonly ICurrencyService _currencyService;

        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;

        private Currency _cachedCurrency;

        public WorkCurrency(ISettingService settingService,
                           ICurrencyService currencyService,
                           IWorkContext workContext)
        {            
            _settingService = settingService;
            _currencyService = currencyService;
            _workContext = workContext;
        }

        public Currency WorkingCurrency
        {
            get
            {
                //whether there is a cached value
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                //return primary store currency when we're in admin area/mode
                /*if (IsAdmin)
                {
                    var primaryStoreCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
                    if (primaryStoreCurrency != null)
                    {
                        _cachedCurrency = primaryStoreCurrency;
                        return primaryStoreCurrency;
                    }
                }*/

                //find a currency previously selected by a customer
                /*var customerCurrencyId = _genericAttributeService.GetAttribute<int>(CurrentCustomer,
                    NopCustomerDefaults.CurrencyIdAttribute, _storeContext.CurrentStore.Id);*/

                int.TryParse(_settingService.GetSettingValueAsync("CurrencyId").Result, out var customerCurrencyId);


                var allStoreCurrencies = _currencyService.GetAllCurrencies(/*storeId: _storeContext.CurrentStore.Id*/);

                //check customer currency availability
                var customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == customerCurrencyId);
                if (customerCurrency == null)
                {

                    var selectedCulture = _workContext.GetCurrentUser().Result.Culture;
                    var regionInfo = new System.Globalization.RegionInfo(selectedCulture);
                    var currencyId = regionInfo.ISOCurrencySymbol;

                    //it not found, then try to get the default currency for the current language (if specified)
                    //customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == WorkingLanguage.DefaultCurrencyId);
                    customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.CurrencyCode == currencyId);
                }

                //if the default currency for the current store not found, then try to get the first one
                if (customerCurrency == null)
                    customerCurrency = allStoreCurrencies.FirstOrDefault();

                //if there are no currencies for the current store try to get the first one regardless of the store
                if (customerCurrency == null)
                    customerCurrency = _currencyService.GetAllCurrencies().FirstOrDefault();

                //cache the found currency
                _cachedCurrency = customerCurrency;

                return _cachedCurrency;
            }
            set
            {
                //get passed currency identifier
                var currencyId = value?.Id ?? 0;

                //and save it
                //_genericAttributeService.SaveAttribute(CurrentCustomer,
                //    NopCustomerDefaults.CurrencyIdAttribute, currencyId, _storeContext.CurrentStore.Id);
                _settingService.UpdateSettingAsync("CurrencyId", currencyId.ToString());

                //then reset the cached value
                _cachedCurrency = null;
            }
        }
    }
}
