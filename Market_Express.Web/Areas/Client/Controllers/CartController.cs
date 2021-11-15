using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Cart;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Web.Controllers;
using Market_Express.Web.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService,
                              IOrderService orderService,
                              IAddressService addressService,
                              IMapper mapper)
        {
            _cartService = cartService;
            _orderService = orderService;
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CartIndexViewModel oViewModel = new();

            await CreateCartIndexViewModel(oViewModel);

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOrderWithNewAddress(string address)
        {
            Address oAddress = new() { Detail = address };

            var oResult = await _addressService.CreateFromCart(oAddress, CurrentUserId);

            if (!oResult.Success)
            {
                TempData["ErrorOrderGenerationResult"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(GenerateOrder));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateOrder()
        {
            var oResult = await _orderService.Generate(CurrentUserId);

            if (!oResult.Success)
            {
                if (oResult.ResultCode == 1)
                    TempData["MustCreateAddress"] = true;
                else
                    TempData["ErrorOrderGenerationResult"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessOrderGenerationResult"] = oResult.Message;

            return Redirect($"/Client/Order/Details/{oResult.Data}");
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> GenerateCart(Guid orderId)
        {
            var oResult = await _cartService.GenerateCartByOrderId(orderId, CurrentUserId);

            if (oResult.Success)
                TempData["CartGenerationResult"] = oResult.Message;

            return Ok(oResult);
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
        public async Task<IActionResult> UpdateDetail(bool plus, Guid articleId, bool fromCartView)
        {
            if (!IsAuthenticated)
                return Unauthorized();

            var oResult = await _cartService.UpdateDetail(plus, articleId, CurrentUserId);

            if (!fromCartView)
                return Ok(oResult);

            if (oResult.Success)
                return await PartialCartIndexResult();
            else
                return Ok(oResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteDetail(Guid articleId, bool fromCartView)
        {
            if (!IsAuthenticated)
                return Unauthorized();

            var oResult = await _cartService.DeleteDetail(articleId, CurrentUserId);

            if (!fromCartView)
                return Ok(oResult);

            if (oResult.Success)
                return await PartialCartIndexResult();
            else
                return Ok(oResult);
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
        #endregion

        #region UTILITY METHODS
        private async Task<IActionResult> PartialCartIndexResult()
        {
            CartIndexViewModel oViewModel = new();

            await CreateCartIndexViewModel(oViewModel);

            return PartialView("_CartDetailsPartial", oViewModel);
        }

        private async Task CreateCartIndexViewModel(CartIndexViewModel oViewModel)
        {
            var tplCartDetails = await _cartService.GetCartDetails(CurrentUserId);

            oViewModel.Cart = _mapper.Map<CartBillingDetailsDTO>(tplCartDetails.Item1);
            oViewModel.Articles = tplCartDetails.Item2.Select(a => _mapper.Map<ArticleForCartDetailsDTO>(a)).ToList();
        }
        #endregion
    }
}
