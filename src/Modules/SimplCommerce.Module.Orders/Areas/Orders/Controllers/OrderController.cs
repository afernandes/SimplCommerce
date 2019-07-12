using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Extensions;
using SimplCommerce.Module.Core.Services;
using SimplCommerce.Module.Orders.Areas.Orders.ViewModels;
using SimplCommerce.Module.Orders.Models;

namespace SimplCommerce.Module.Orders.Areas.Orders.Controllers
{
    [Area("Orders")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IWorkContext _workContext;
        private readonly IWorkCurrency _workCurrency;
        private readonly ICurrencyService _currencyService;

        public OrderController(IRepository<Order> orderRepository, IWorkContext workContext, IWorkCurrency workCurrency, IMediaService mediaService, ICurrencyService currencyService)
        {
            _orderRepository = orderRepository;
            _workContext = workContext;
            _workCurrency = workCurrency;
            _mediaService = mediaService;
            _currencyService = currencyService;
        }

        [HttpGet("user/order-history")]
        public async Task<IActionResult> OrderHistoryList()
        {
            var user = await _workContext.GetCurrentUser();
            var model = await _orderRepository
                .Query()
                .Where(x => x.CustomerId == user.Id && x.ParentId == null)
                .Select(x => new OrderHistoryListItem
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    SubTotal = x.SubTotal,
                    OrderStatus = x.OrderStatus,
                    OrderItems = x.OrderItems.Select(i => new OrderHistoryProductVm
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        ThumbnailImage = i.Product.ThumbnailImage.FileName,
                        ProductOptions = i.Product.OptionCombinations.Select(o => o.Value)
                    }).ToList()
                })
                .OrderByDescending(x => x.CreatedOn).ToListAsync();

            foreach (var item in model)
            {
                foreach (var product in item.OrderItems)
                {
                    product.ThumbnailImage = _mediaService.GetMediaUrl(product.ThumbnailImage);
                }
            }

            return View(model);
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
