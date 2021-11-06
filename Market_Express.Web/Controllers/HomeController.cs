using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Home;
using Market_Express.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(IHomeService homeService,
                              ICategoryService categoryService,
                              IArticleService articleService,
                              IMapper mapper)
        {
            _homeService = homeService;
            _categoryService = categoryService;
            _articleService = articleService;
            _mapper = mapper;
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

            oViewModel.Sliders = GetSliderDTOList();
            oViewModel.Categories = GetCategoryDTOList();
            oViewModel.Articles = GetArticleDTOList();

            return View(oViewModel);
        }

        #region UTILITY METHODS
        private async Task<(List<ArticleDTO>, Metadata)> GetArticleDTOListAndMetaForSearch(HomeSearchQueryFilter filters)
        {
            var lstPagedArticles = await _articleService.GetAllForSearch(filters);

            var oMeta = Metadata.Create(lstPagedArticles);

            return (lstPagedArticles.Select(article => _mapper.Map<ArticleDTO>(article))
                                 .ToList()
                    , oMeta);
        }

        private async Task<List<CategorySearchDTO>> GetCategorySearchDTOList()
        {
            return (await _categoryService.GetAllAvailableForSearch())
                                          .Select(cat => _mapper.Map<CategorySearchDTO>(cat))
                                          .ToList();
        }

        private List<CategoryDTO> GetCategoryDTOList()
        {
            return _categoryService.GetAllAvailable()
                                   .Select(cat => _mapper.Map<CategoryDTO>(cat))
                                   .ToList();
        }

        private List<SliderDTO> GetSliderDTOList()
        {
            return _homeService.GetAllSliders()
                               .Select(slider => _mapper.Map<SliderDTO>(slider))
                               .ToList();
        }

        private List<ArticleDTO> GetArticleDTOList()
        {
            return _articleService.GetAllActive(20)
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
        #endregion

        #endregion
    }
}
