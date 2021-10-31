using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Article;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public PagedList<Article> GetAll(ArticleIndexQueryFilter filters, bool includeCategory = false)
        {
            IEnumerable<Article> lstArticles;

            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            if(includeCategory)
                lstArticles = _unitOfWork.Article.GetAll(nameof(Article.Category));
            else
                lstArticles = _unitOfWork.Article.GetAll();

            if (filters.Description != null)
                lstArticles = lstArticles.Where(article => article.Description.Trim().ToUpper().Contains(filters.Description.Trim().ToUpper()));

            if (filters.BarCode != null)
                lstArticles = lstArticles.Where(article => article.BarCode.Trim().ToUpper().Contains(filters.BarCode.Trim().ToUpper()));

            if (filters.CategoryId != null)
            {
                if (filters.CategoryId == Guid.Empty)
                    lstArticles = lstArticles.Where(article => article.CategoryId == null);
                else
                    lstArticles = lstArticles.Where(article => article.CategoryId == filters.CategoryId);
            }

            if (filters.CategoryIdIsNull)
                lstArticles = lstArticles.Where(article => article.CategoryId is null);

            if (filters.ImgIsNull)
                lstArticles = lstArticles.Where(article => article.Image is null);

            if (filters.Status != null)
                lstArticles = lstArticles.Where(article => article.Status == filters.Status);

            var pagedArticles = PagedList<Article>.Create(lstArticles, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedArticles;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid articleId, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            var oArticle = await _unitOfWork.Article.GetByIdAsync(articleId);

            if (oArticle == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            int iCartCount = await _unitOfWork.Cart.GetOpenCountByArticleId(articleId);

            if(iCartCount > 0)
            {
                oResult.Message = $"El artículo no se puede desactivar porque está agregado en el carrito de {iCartCount} clientes.";

                return oResult;
            }

            if (oArticle.Status == EntityStatus.ACTIVADO)
            {
                oArticle.Status = EntityStatus.DESACTIVADO;

                oResult.ResultCode = 0;

                oResult.Message = "El artículo se ha desactivado.";
            }
            else
            {
                oArticle.Status = EntityStatus.ACTIVADO;

                oResult.ResultCode = 1;

                oResult.Message = "El artículo se ha activado.";
            }

            oArticle.ModificationDate = DateTimeUtility.NowCostaRica;
            oArticle.ModifiedBy = currentUserId.ToString();

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }
    }    
}
