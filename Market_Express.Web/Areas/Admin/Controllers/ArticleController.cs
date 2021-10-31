using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Article;
using Market_Express.Web.Controllers;
using Market_Express.Web.ViewModels.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "ART_MAN_GEN")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService,
                                 ICategoryService categoryService,
                                 IMapper mapper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(ArticleIndexQueryFilter filters)
        {
            ArticleIndexViewModel oViewModel = new();

            var tplPaginatedArticles = GetMetaAndPagedArticleDTOList(filters);

            oViewModel.Articles = tplPaginatedArticles.Item1;
            oViewModel.Metadata = tplPaginatedArticles.Item2;
            oViewModel.AvailableCategories = GetAvailableCategories();
            oViewModel.QueryFilter = filters;

            return View(oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Article/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ArticleEditViewModel oViewModel = new();

            oViewModel.Article = _mapper.Map<ArticleEditDTO>(await _articleService.GetById(id));
            oViewModel.AvailableCategories = GetAvailableCategories();

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditDTO model)
        {
            var oResult = new { Success = false, Message = "Algo salio mal" };

            if (!oResult.Success)
            {
                ArticleEditViewModel oViewModel = new();

                oViewModel.Article = model;
                oViewModel.AvailableCategories = GetAvailableCategories();

                ViewData["ResultMessage"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["ArticleMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetArticleTable(ArticleIndexQueryFilter filters)
        {
            ArticleIndexViewModel oViewModel = new();

            var paginationResult = GetMetaAndPagedArticleDTOList(filters);

            oViewModel.Articles = paginationResult.Item1;
            oViewModel.Metadata = paginationResult.Item2;
            oViewModel.QueryFilter = filters;

            return PartialView("_ArticleTablePartial", oViewModel);
        }

        #region UTILITY METHODS
        private (List<ArticleDTO>, Metadata) GetMetaAndPagedArticleDTOList(ArticleIndexQueryFilter filters)
        {
            var pagedArticles = _articleService.GetAll(filters, true);
            var oMeta = Metadata.Create(pagedArticles);

            return (pagedArticles.Select(article => _mapper.Map<ArticleDTO>(article))
                                 .ToList()
                    , oMeta);
        }

        private List<CategoryDTO> GetAvailableCategories()
        {
            return _categoryService.GetAllAvailable()
                                   .Select(cat => _mapper.Map<CategoryDTO>(cat))
                                   .ToList();
        }
        #endregion

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromQuery(Name = "id")] Guid id)
        {
            var oResult = await _articleService.ChangeStatus(id, CurrentUserId);

            return Ok(oResult);
        }
        #endregion
    }
}
