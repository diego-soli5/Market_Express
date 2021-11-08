using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Cart;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Article;
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
        private readonly IMapper _mapper;

        public CartController(ICartService cartService,
                              IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CartIndexViewModel oViewModel = new();

            var tplCartDetails = await _cartService.GetCartDetails(CurrentUserId);

            oViewModel.Cart = _mapper.Map<CartBillingDetailsDTO>(tplCartDetails.Item1);
            oViewModel.Articles = tplCartDetails.Item2.Select(a => _mapper.Map<ArticleForCartDetailsDTO>(a)).ToList();

            return View(oViewModel);
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

            var oResult = await _cartService.UpdateDetail(plus, articleId, CurrentUserId);

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
    }
}
