using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Infrastructure;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Core.Services;

namespace SimplCommerce.Module.Core.Extensions
{
    public partial class WorkContext : IWorkContext
    {
        private const string UserGuidCookiesName = "SimplUserGuid";
        private const long GuestRoleId = 3;

        private readonly UserManager<User> _userManager;
        private readonly HttpContext _httpContext;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrencyService _currencyService;

        private readonly ISettingService _settingService;

        private User _currentUser;
        private Currency _cachedCurrency;

        public WorkContext(UserManager<User> userManager,
                           IHttpContextAccessor contextAccessor,
                           IRepository<User> userRepository,
                           ISettingService settingService,
                           ICurrencyService currencyService)
        {
            _userManager = userManager;
            _httpContext = contextAccessor.HttpContext;
            _userRepository = userRepository;
            _settingService = settingService;
            _currencyService = currencyService;
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


                var allStoreCurrencies = _currencyService.GetAllCurrencies(storeId: _storeContext.CurrentStore.Id);

                //check customer currency availability
                var customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == customerCurrencyId);
                if (customerCurrency == null)
                {
                    //it not found, then try to get the default currency for the current language (if specified)
                    customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == WorkingLanguage.DefaultCurrencyId);
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
                _genericAttributeService.SaveAttribute(CurrentCustomer,
                    NopCustomerDefaults.CurrencyIdAttribute, currencyId, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedCurrency = null;
            }
        }

        public async Task<User> GetCurrentUser()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            var contextUser = _httpContext.User;
            _currentUser = await _userManager.GetUserAsync(contextUser);

            if (_currentUser != null)
            {
                return _currentUser;
            }

            var userGuid = GetUserGuidFromCookies();
            if (userGuid.HasValue)
            {
                _currentUser = _userRepository.Query().Include(x => x.Roles).FirstOrDefault(x => x.UserGuid == userGuid);
            }

            if (_currentUser != null && _currentUser.Roles.Count == 1 && _currentUser.Roles.First().RoleId == GuestRoleId)
            {
                return _currentUser;
            }

            userGuid = Guid.NewGuid();
            var dummyEmail = string.Format("{0}@guest.simplcommerce.com", userGuid);
            _currentUser = new User
            {
                FullName = "Guest",
                UserGuid = userGuid.Value,
                Email = dummyEmail,
                UserName = dummyEmail,
                Culture = GlobalConfiguration.DefaultCulture
            };
            var abc = await _userManager.CreateAsync(_currentUser, "1qazZAQ!");
            await _userManager.AddToRoleAsync(_currentUser, "guest");
            SetUserGuidCookies(_currentUser.UserGuid);
            return _currentUser;
        }

        private Guid? GetUserGuidFromCookies()
        {
            if (_httpContext.Request.Cookies.ContainsKey(UserGuidCookiesName))
            {
                return Guid.Parse(_httpContext.Request.Cookies[UserGuidCookiesName]);
            }

            return null;
        }

        private void SetUserGuidCookies(Guid userGuid)
        {
            _httpContext.Response.Cookies.Append(UserGuidCookiesName, _currentUser.UserGuid.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(5),
                HttpOnly = true,
                IsEssential = true
            });
        }
    }
}
