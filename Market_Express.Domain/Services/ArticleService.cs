﻿using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.Domain.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Article;
using Market_Express.Domain.QueryFilter.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobStorageService _blobService;
        private readonly PaginationOptions _paginationOptions;
        private readonly ArticleOptions _articleOptions;

        public ArticleService(IUnitOfWork unitOfWork,
                              IAzureBlobStorageService blobService,
                              IOptions<PaginationOptions> paginationOptions,
                              IOptions<ArticleOptions> articleOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _paginationOptions = paginationOptions.Value;
            _articleOptions = articleOptions.Value;
        }

        public PagedList<Article> GetAll(ArticleIndexQueryFilter filters, bool includeCategory = false)
        {
            IQueryable<Article> lstArticles;

            CheckPaginationFilters(filters);

            if (includeCategory)
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
                lstArticles = lstArticles.Where(article => article.CategoryId == null);

            if (filters.ImgIsNull)
                lstArticles = lstArticles.Where(article => article.Image == null);

            if (filters.Status != null)
                lstArticles = lstArticles.Where(article => article.Status == filters.Status);

            var pagedArticles = PagedList<Article>.Create(lstArticles, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedArticles;
        }

        public async Task<SQLServerPagedList<ArticleToAddInCart>> GetAllForSell(HomeSearchQueryFilter filters, Guid? userId)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultClientArticleSearchPageSize;
            filters.PageSize = filters.PageSize > 24 ? 24 : filters.PageSize;

            var lstArticles = await _unitOfWork.Article.GetAllForSellPaginated(filters, userId);

            return lstArticles;
        }

        public async Task<ArticleToAddInCart> GetByIdForSell(Guid articleId, Guid? userId)
        {
            var oArticle = await _unitOfWork.Article.GetByIdForSell(articleId, userId);

            if (oArticle == null)
                throw new NotFoundException(articleId, nameof(Article));

            if (oArticle.Status == EntityStatus.DESACTIVADO && oArticle.Category.Status == EntityStatus.DESACTIVADO)
                throw new NotFoundException();

            return oArticle;
        }

        public async Task<List<Article>> GetMostPopular(int? take = null)
        {
            return await _unitOfWork.Article.GetMostPopular(take);
        }

        public IEnumerable<Article> GetAllActive(int? max = null)
        {
            var lstArticle = _unitOfWork.Article.GetAllActiveWithCategoryAsigned();

            if (max.HasValue)
                lstArticle = lstArticle.Take(max.Value);

            return lstArticle;
        }

        public async Task<Article> GetById(Guid id, bool includeCategory = false)
        {
            Article oArticle;
            if (includeCategory)
                oArticle = await _unitOfWork.Article.GetByIdAsync(id, nameof(Article.Category));
            else
                oArticle = await _unitOfWork.Article.GetByIdAsync(id);

            if (oArticle == null)
                throw new NotFoundException(id, nameof(Article));

            return oArticle;
        }

        public async Task<BusisnessResult> Create(Article article, IFormFile image, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if (!await ValidateArticle(oResult, article))
                return oResult;

            if (image?.Length > 0)
            {
                if (!IsValidImage(image))
                {
                    oResult.Message = "El formato de imagen es invalido.";

                    oResult.ResultCode = 0;

                    return oResult;
                }

                article.Image = await _blobService.CreateBlobAsync(image);
            }

            article.AutoSync = false;
            article.AutoSyncDescription = false;
            article.Status = EntityStatus.ACTIVADO;
            article.CreationDate = DateTimeUtility.NowCostaRica;
            article.AddedBy = currentUserId.ToString();

            _unitOfWork.Article.Create(article);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "El artículo se creó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> Edit(Article article, IFormFile image, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if (!await ValidateArticle(oResult, article, true))
                return oResult;

            var oArticleFromDb = await _unitOfWork.Article.GetByIdAsync(article.Id);

            if (oArticleFromDb == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            int iCartCount = await _unitOfWork.Cart.GetOpenCountByArticleId(article.Id);

            if (iCartCount > 0)
            {
                oResult.Message = $"El artículo no se puede desactivar porque está agregado en {iCartCount} carritos.";

                return oResult;
            }

            if (image?.Length > 0)
            {
                if (!IsValidImage(image))
                {
                    oResult.Message = "El formato de imagen es invalido.";

                    oResult.ResultCode = 0;

                    return oResult;
                }

                await _blobService.DeleteBlobAsync(oArticleFromDb.Image ?? "");

                oArticleFromDb.Image = await _blobService.CreateBlobAsync(image);
            }

            oArticleFromDb.CategoryId = article.CategoryId;
            oArticleFromDb.Description = article.Description;
            oArticleFromDb.BarCode = article.BarCode;
            oArticleFromDb.Price = article.Price;
            oArticleFromDb.Status = article.Status;
            oArticleFromDb.ModificationDate = DateTimeUtility.NowCostaRica;
            oArticleFromDb.ModifiedBy = currentUserId.ToString();

            if (oArticleFromDb.AddedBy == "SYSTEM")
            {
                oArticleFromDb.AutoSync = article.AutoSync;
                oArticleFromDb.AutoSyncDescription = article.AutoSyncDescription;
            }

            oResult.Success = await _unitOfWork.Save();

            if (oArticleFromDb.AutoSync && !oArticleFromDb.AutoSyncDescription && oArticleFromDb.AddedBy == "SYSTEM")
                oResult.Message = "El artículo se modificó correctamente! Sin embargo los cambios se pueden perder (exceptuando la descripción) porque la sincronización automática está activada para este artículo.";

            else if (oArticleFromDb.AutoSync && oArticleFromDb.AddedBy == "SYSTEM")
                oResult.Message = "El artículo se modificó correctamente! Sin embargo los cambios se pueden perder porque la sincronización automática está activada para este artículo.";

            else
                oResult.Message = "El artículo se modificó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid articleId, bool enableCategory, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            var oArticle = await _unitOfWork.Article.GetByIdAsync(articleId, nameof(Article.Category));

            if (oArticle == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            int iCartCount = await _unitOfWork.Cart.GetOpenCountByArticleId(articleId);

            if (iCartCount > 0)
            {
                oResult.Message = $"El artículo no se puede desactivar porque está agregado en {iCartCount} carritos.";

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
                if (oArticle.Category != null)
                {
                    if (oArticle.Category.Status == EntityStatus.ACTIVADO)
                    {
                        oArticle.Status = EntityStatus.ACTIVADO;

                        oResult.ResultCode = 1;

                        oResult.Message = "El artículo se ha activado.";
                    }
                    else
                    {
                        if (enableCategory)
                        {
                            oArticle.Status = EntityStatus.ACTIVADO;

                            oArticle.Category.Status = EntityStatus.ACTIVADO;

                            oResult.ResultCode = 1;

                            oResult.Message = "El artículo y la categoría se se han activado.";

                            _unitOfWork.Category.Update(oArticle.Category);
                        }
                        else
                        {
                            oResult.Message = "No se puedo activar el artículo porque la categoría está desactivada.";

                            oResult.ResultCode = 2;

                            return oResult;
                        }
                    }
                }
                else
                {
                    oArticle.Status = EntityStatus.ACTIVADO;

                    oResult.ResultCode = 1;

                    oResult.Message = "El artículo se ha activado pero no se mostrará en resultados hasta que se le asigne una categoría.";
                }
            }

            oArticle.ModificationDate = DateTimeUtility.NowCostaRica;
            oArticle.ModifiedBy = currentUserId.ToString();

            _unitOfWork.Article.Update(oArticle);

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }

        public async Task<BusisnessResult> SetCategory(Guid articleId, Guid categoryId, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            var oArticle = await _unitOfWork.Article.GetByIdAsync(articleId);

            if (oArticle == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            var oCategory = await _unitOfWork.Category.GetByIdAsync(categoryId);

            if (oCategory == null)
            {
                oResult.Message = "La categoría no existe.";

                return oResult;
            }

            oArticle.CategoryId = categoryId;
            oArticle.ModificationDate = DateTimeUtility.NowCostaRica;
            oArticle.ModifiedBy = currentUserId.ToString();

            _unitOfWork.Article.Update(oArticle);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "La categoría se asignó al artículo.";

            oResult.Data = oCategory.Name;

            return oResult;
        }

        #region UTILITY METHODS
        private async Task<bool> ValidateArticle(BusisnessResult result, Article article, bool isEdit = false)
        {
            if (string.IsNullOrEmpty(article.Description))
            {
                result.Message = "El campo descripción es obligatorio.";

                return false;
            }
            else if (article.Description.Length > 255)
            {
                result.Message = "El campo descripción no puede superar los 255 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(article.BarCode))
            {
                result.Message = "El campo código de barras es obligatorio.";

                return false;
            }
            else if (article.BarCode.Length > 255)
            {
                result.Message = "El campo código de barras no puede superar los 255 caracteres.";

                return false;
            }

            if (article.CategoryId == null)
            {
                result.Message = "Se debe seleccionar una categoría.";

                return false;
            }

            var oArticleToValidate = _unitOfWork.Article.GetFirstOrDefault(art => art.BarCode.Trim().ToUpper() == article.BarCode.Trim().ToUpper());

            if (oArticleToValidate != null)
            {
                if (isEdit)
                {
                    if (oArticleToValidate.Id != article.Id)
                    {
                        result.Message = "El código de barras ya existe.";

                        return false;
                    }
                }
                else
                {
                    result.Message = "El código de barras ya existe.";

                    return false;
                }
            }

            var oCategoryToValidate = await _unitOfWork.Category.GetByIdAsync(article.CategoryId.Value);

            if (oCategoryToValidate == null)
            {
                result.Message = "La categoría no existe.";

                return false;
            }

            return true;
        }
        #endregion
    }
}
