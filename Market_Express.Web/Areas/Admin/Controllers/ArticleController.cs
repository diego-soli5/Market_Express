using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Article;
using Market_Express.Web.ViewModels.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "ART_MAN_GEN")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService,
                                 IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(ArticleIndexQueryFilter filters)
        {
            ArticleIndexViewModel oViewModel = new();

            var paginationResult = GetMetaAndPagedArticleDTOList(filters);

            oViewModel.Articles = paginationResult.Item1;
            oViewModel.Metadata = paginationResult.Item2;
            oViewModel.QueryFilter = filters;

            return View(oViewModel);
        }

        #region UTILITY METHODS
        private (List<ArticleDTO>, Metadata) GetMetaAndPagedArticleDTOList(ArticleIndexQueryFilter filters)
        {
            var pagedArticles = _articleService.GetAll(filters);
            var oMeta = Metadata.Create(pagedArticles);

            return (pagedArticles.Select(article => _mapper.Map<ArticleDTO>(article))
                                 .ToList()
                    , oMeta);
        }
        #endregion
    }
}
