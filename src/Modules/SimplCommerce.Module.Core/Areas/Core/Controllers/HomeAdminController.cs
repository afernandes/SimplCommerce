using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimplCommerce.Module.Core.Areas.Core.Controllers
{
    [Area("Core")]
    [Authorize(Roles = "admin, vendor")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeAdminController : Controller
    {
        private readonly IAntiforgery _antiforgery;

        public HomeAdminController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [Route("admin")]
        public IActionResult Index()
        { 
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            HttpContext.Response.Cookies.Append("XSRF-TOKEN",
<<<<<<< HEAD
                tokens.RequestToken, new CookieOptions { HttpOnly = false, IsEssential = true }
=======
                tokens.RequestToken, new CookieOptions { HttpOnly = false }
>>>>>>> origin/master
            );

            return View();
        }
    }
}
