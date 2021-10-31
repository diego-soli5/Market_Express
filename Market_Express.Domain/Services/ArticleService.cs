using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Article;
using Microsoft.Extensions.Options;
using System.Linq;


namespace Market_Express.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public ArticleService(IUnitOfWork unitOfWork,
                              IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public PagedList<Article> GetAll(ArticleIndexQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            var lstArticles = _unitOfWork.Article.GetAll();

            if (filters.Description != null)
                lstArticles = lstArticles.Where(article => article.Description.Trim().ToUpper().Contains(filters.Description.Trim().ToUpper()));

            if (filters.BarCode != null)
                lstArticles = lstArticles.Where(article => article.BarCode.Trim().ToUpper().Contains(filters.BarCode.Trim().ToUpper()));

            if (filters.CategoryId != null)
                lstArticles = lstArticles.Where(article => article.CategoryId == filters.CategoryId);

            if (filters.CategoryIdIsNull)
                lstArticles = lstArticles.Where(article => article.CategoryId is null);

            if (filters.ImgIsNull)
                lstArticles = lstArticles.Where(article => article.Image is null);

            if (filters.Status != null)
                lstArticles = lstArticles.Where(article => article.Status == filters.Status);

            var pagedArticles = PagedList<Article>.Create(lstArticles, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedArticles;
        }
    }
}
