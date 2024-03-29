﻿using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Home;
using Market_Express.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly CategoryOptions _categoryOptions;
        private readonly ArticleOptions _articleOptions;

        public HomeController(IHomeService homeService,
                              ICategoryService categoryService,
                              IArticleService articleService,
                              IMapper mapper,
                              IOptions<CategoryOptions> categoryOptions,
                              IOptions<ArticleOptions> articleOptions)
        {
            _homeService = homeService;
            _categoryService = categoryService;
            _articleService = articleService;
            _mapper = mapper;
            _categoryOptions = categoryOptions.Value;
            _articleOptions = articleOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(HomeSearchQueryFilter filters)
        {
            if (filters != null)
            {
                if (HasFilters(filters))
                {
                    HomeSearchViewModel oViewModelSearch = new();

                    oViewModelSearch.Categories = await GetCategorySearchDTOList();
                    oViewModelSearch.Filters = filters;

                    if (filters.FromSearchView.HasValue)
                    {
                        if (!filters.FromSearchView.Value)
                        {
                            if (oViewModelSearch.Filters.Category == null)
                            {
                                oViewModelSearch.Filters.Category = new();

                                oViewModelSearch.Categories.ForEach(cat =>
                                {
                                    oViewModelSearch.Filters.Category.Add(cat.Id);
                                });
                            }
                        }
                    }

                    var tplPaginatedArticles = await GetArticleDTOListAndMetaForSearch(filters);

                    oViewModelSearch.Articles = tplPaginatedArticles.Item1;
                    oViewModelSearch.Metadata = tplPaginatedArticles.Item2;

                    return View("Search", oViewModelSearch);
                }
            }

            HomeViewModel oViewModel = new();

            oViewModel.PopularArticles = await GetMostPopularArticleDTOList();
            oViewModel.PopularCategories = await GetMostPopularCategoryDTOList();
            oViewModel.Sliders = GetSliderDTOList();

            return View(oViewModel);
        }

        public async Task<IActionResult> Search(HomeSearchQueryFilter filters)
        {
            HomeSearchViewModel oViewModel = new();

            var tplPaginatedArticles = await GetArticleDTOListAndMetaForSearch(filters);

            oViewModel.Articles = tplPaginatedArticles.Item1;
            oViewModel.Metadata = tplPaginatedArticles.Item2;
            oViewModel.Filters = filters;

            return PartialView("_ArticlesSearchResultPartial", oViewModel);
        }

        [HttpGet]
        [Route("/Article/{id}")]
        public async Task<IActionResult> Article(Guid id, bool? refresh)
        {
            Guid? userId = null;

            if (IsAuthenticated)
                userId = CurrentUserId;

            var oArticle = _mapper.Map<ArticleToAddInCartDTO>(await _articleService.GetByIdForSell(id, userId));

            if (refresh.HasValue)
                if (refresh.Value)
                    return PartialView("_ArticleCartButtonsPartial", oArticle);

            return View(oArticle);
        }

        [HttpGet]
        [Route("/Categories")]
        public IActionResult Categories()
        {
            var lstCategories = _categoryService.GetAllActive()
                                                .Select(c => _mapper.Map<CategoryDTO>(c))
                                                .ToList();

            return View(lstCategories);
        }

        #region UTILITY METHODS
        private async Task<(List<ArticleToAddInCartDTO>, Metadata)> GetArticleDTOListAndMetaForSearch(HomeSearchQueryFilter filters)
        {
            Guid? userId = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = CurrentUserId;
            }

            var lstPagedArticles = await _articleService.GetAllForSell(filters, userId);

            var oMeta = Metadata.Create(lstPagedArticles);

            return (lstPagedArticles.Select(article => _mapper.Map<ArticleToAddInCartDTO>(article))
                                    .ToList()
                    , oMeta);
        }

        private async Task<List<CategorySearchDTO>> GetCategorySearchDTOList()
        {
            return (await _categoryService.GetAllAvailableForSearch())
                                          .Select(cat => _mapper.Map<CategorySearchDTO>(cat))
                                          .ToList();
        }

        private async Task<List<CategoryDTO>> GetMostPopularCategoryDTOList()
        {
            return (await _categoryService.GetMostPopular(_categoryOptions.DefaultTakeForMostPopular)) //3
                                          .Select(cat => _mapper.Map<CategoryDTO>(cat))
                                          .ToList();
        }

        private List<SliderDTO> GetSliderDTOList()
        {
            return _homeService.GetAllSliders()
                               .Select(slider => _mapper.Map<SliderDTO>(slider))
                               .ToList();
        }

        private async Task<List<ArticleDTO>> GetMostPopularArticleDTOList()
        {
            return (await _articleService.GetMostPopular(_articleOptions.DefaultTakeForMostPopular)) //20
                                         .Select(article => _mapper.Map<ArticleDTO>(article))
                                         .ToList();
        }

        private bool HasFilters(HomeSearchQueryFilter filters)
        {
            return filters.GetType()
                          .GetProperties()
                          .Any(p => p.GetValue(filters) != null);
        }
        #endregion

        #region MISCELLANEOUS VIEWS

        #region COMPANY VIEWS
        [HttpGet]
        [Route("/Home/Company/About")]
        public IActionResult AboutCompany()
        {
            return View();
        }
        #endregion

        #region HELP VIEWS
        [HttpGet]
        [Route("/Home/Help/Contact")]
        public IActionResult ContactHelp()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/Help/UserManual")]
        public IActionResult UserManual()
        {
            return View();
        }
        #endregion

        #region SYSTEM VIEWS
        [HttpGet]
        [Route("/Home/System/About")]
        public IActionResult AboutSystem()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/System/Contact")]
        public IActionResult ContactSystem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessageToDeveloper(string name, string phone, string email, string message)
        {
            var oResult = _homeService.SendMessageToDeveloper(name, phone, email, message);

            TempData["MessageResult"] = oResult.Message;

            return RedirectToAction(nameof(ContactSystem));
        }
        #endregion

        #endregion
    }
}
