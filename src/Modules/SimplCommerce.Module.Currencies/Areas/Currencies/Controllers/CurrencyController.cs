using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimplCommerce.Module.Core.Extensions;
using SimplCommerce.Module.Core.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplCommerce.Module.Currencies.Areas.Currencies.Controllers
{
    [Area("Currencies")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CurrencyController : Controller
    {
        #region Fields

        private readonly ICurrencyService _currencyService;
        private readonly IWorkCurrency _workCurrency;

        #endregion

        #region Ctor

        public CurrencyController(ICurrencyService currencyService,
            IWorkCurrency workCurrency)
        {
            _currencyService = currencyService;
            _workCurrency = workCurrency;
        }

        #endregion


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult SetCurrency(int customerCurrency, string returnUrl = "")
        {
            var currency = _currencyService.GetCurrencyById(customerCurrency);
            if (currency != null)
                _workCurrency.WorkingCurrency = currency;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

    }
}
