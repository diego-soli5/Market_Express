using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddDetail(Guid articleId)
        {
            if (!IsAuthenticated)
                return Unauthorized();

            var oResult = await _cartService.AddDetail(articleId, CurrentUserId);

            return Ok(oResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDetail(bool plus, Guid articleId)
        {
            if (!IsAuthenticated)
                return Unauthorized();

            if (plus)
            {
                var oResult = new
                {
                    Success = true,
                    Message = "",
                    ResultCode = 0,
                    Data = 2
                };
                return Ok(oResult);
            }
            else
            {
                var oResult = new
                {
                    Success = true,
                    Message = "",
                    ResultCode = 0,
                    Data = 0
                };
                return Ok(oResult);
            }


            return Ok();
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
