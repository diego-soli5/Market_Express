using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartArticlesCount()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("0");

            int iCount = await _cartService.GetArticlesCount(CurrentUserId);

            return Content(iCount.ToString());
        }
    }
}
