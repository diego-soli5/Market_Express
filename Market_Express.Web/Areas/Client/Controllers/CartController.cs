using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> GetCartArticlesCount()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("0");

            int iCount = await _cartService.GetArticlesCount(GetCurrentUserId());

            return Content(iCount.ToString());
        }

        #region HELPER METHODS
        private Guid GetCurrentUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return new Guid(id);
        }
        #endregion
    }
}
