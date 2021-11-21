using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        private const string _Sp_Article_GetAllForSell = "Sp_Article_GetAllForSell";
        private const string _Sp_Article_GetMostPopular = "Sp_Article_GetMostPopular";
        private const string _Sp_Article_GetAllForCartDetails = "Sp_Article_GetAllForCartDetails";
        private const string _Sp_Article_GetByIdForSell = "Sp_Article_GetByIdForSell";

        public ArticleRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
        { }

        public IQueryable<Article> GetAllActiveWithCategoryAsigned()
        {
            return _dbEntity.Where(article => article.Status == EntityStatus.ACTIVADO && article.CategoryId != null);
        }

        public async Task<SQLServerPagedList<ArticleToAddInCart>> GetAllForSellPaginated(HomeSearchQueryFilter filters, Guid? userId)
        {
            List<ArticleToAddInCart> lstArticles = new();

            SqlParameter pTotalPages = new("@totalPages", 0)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter pTotalCount = new("@totalCount", 0)
            {
                Direction = ParameterDirection.Output
            };

            var arrParams = new[]
            {
                new SqlParameter("@description",filters.Query),
                new SqlParameter("@maxPrice",filters.MaxPrice),
                new SqlParameter("@minPrice",filters.MinPrice),
                new SqlParameter("@category",filters.Category is not null ? string.Join(',', filters.Category) : ""),
                new SqlParameter("@pageNumber",filters.PageNumber-1),
                new SqlParameter("@pageSize",filters.PageSize),
                new SqlParameter("@userId",userId),
                pTotalPages,
                pTotalCount
            };

            var dtResult = await ExecuteQuery(_Sp_Article_GetAllForSell, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ArticleToAddInCart
                {
                    Id = (Guid)oRow["Id"],
                    CategoryId = (Guid)oRow["CategoryId"],
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Price = Convert.ToDecimal(oRow["Price"].ToString()),
                    Image = oRow["Image"] is DBNull ? null : oRow["Image"].ToString(),
                    CountInCart = Convert.ToInt32(oRow["CountInCart"])
                });
            }

            return new SQLServerPagedList<ArticleToAddInCart>(lstArticles, filters.PageNumber.Value, filters.PageSize.Value, Convert.ToInt32(pTotalPages.Value), Convert.ToInt32(pTotalCount.Value));
        }

        public async Task<ArticleToAddInCart> GetByIdForSell(Guid articleId, Guid? userId)
        {
            ArticleToAddInCart oArticle = null;

            var arrParams = new[]
            {
                new SqlParameter("@articleId",articleId),
                new SqlParameter("@userId",userId),
            };

            var dtResult = await ExecuteQuery(_Sp_Article_GetByIdForSell, arrParams);

            if(dtResult?.Rows?.Count > 0)
            {
                var drResult = dtResult.Rows[0];

                oArticle = new()
                {
                    Id = (Guid)drResult["Id"],
                    CategoryId = (Guid)drResult["CategoryId"],
                    Description = drResult["Description"].ToString(),
                    BarCode = drResult["BarCode"].ToString(),
                    Price = Convert.ToDecimal(drResult["Price"].ToString()),
                    Image = drResult["Image"] is DBNull ? null : drResult["Image"].ToString(),
                    Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), drResult["Status"].ToString()),
                    CountInCart = Convert.ToInt32(drResult["CountInCart"]),
                    Category = new()
                    {
                        Id = (Guid)drResult["CategoryId"],
                        Name = drResult["CategoryName"].ToString(),
                        Description = drResult["CategoryDescription"].ToString(),
                        Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), drResult["CategoryStatus"].ToString()),
                        Image = drResult["CategoryImage"] is DBNull ? null : drResult["CategoryImage"].ToString(),
                    }
                };
            }

            return oArticle;
        }

        public async Task<List<Article>> GetMostPopular(int? take = null)
        {
            List<Article> lstArticles = new();

            var arrParams = new[]
            {
                new SqlParameter("@take",take),
            };

            var dtResult = await ExecuteQuery(_Sp_Article_GetMostPopular, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new Article
                {
                    Id = (Guid)oRow["Id"],
                    CategoryId = (Guid)oRow["CategoryId"],
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Price = Convert.ToDecimal(oRow["Price"].ToString()),
                    Image = oRow["Image"] is DBNull ? null : oRow["Image"].ToString()
                });
            }

            return lstArticles;
        }

        public async Task<List<ArticleForCartDetails>> GetAllForCartDetails(Guid userId)
        {
            List<ArticleForCartDetails> lstArticles = new();

            var arrParams = new[]
            {
                new SqlParameter("@userId",userId)
            };

            var dtResult = await ExecuteQuery(_Sp_Article_GetAllForCartDetails, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ArticleForCartDetails
                {
                    Id = (Guid)oRow["Id"],
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Price = Convert.ToDecimal(oRow["Price"].ToString()),
                    Image = oRow["Image"] is DBNull ? null : oRow["Image"].ToString(),
                    Category = new Category
                    {
                        Name = oRow["CategoryName"].ToString()
                    },
                    Quantity = Convert.ToDecimal(oRow["Quantity"].ToString())
                });
            }

            return lstArticles;
        }
    }
}
