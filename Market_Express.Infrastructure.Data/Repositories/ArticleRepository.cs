using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Home;
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
        private const string _Sp_Article_GetAllForSearch = "Sp_Article_GetAllForSearch";

        public ArticleRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public IEnumerable<Article> GetAllActiveWithCategoryAsigned()
        {
            return _dbEntity.Where(article => article.Status == EntityStatus.ACTIVADO && article.CategoryId != null)
                            .AsEnumerable();
        }

        public async Task<SQLServerPagedList<Article>> GetAllForSearch(HomeSearchQueryFilter filters)
        {
            List<Article> lstArticles = new();

            int totalPages = 0;
            int totalCount = 0;

            SqlParameter pTotalPages = new("@totalPages", totalPages)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter pTotalCount = new("@totalCount", totalCount)
            {
                Direction = ParameterDirection.Output
            };

            var arrParams = new[]
            {
                new SqlParameter("@description",filters.Query),
                new SqlParameter("@maxPrice",filters.MaxPrice),
                new SqlParameter("@minPrice",filters.MinPrice),
                new SqlParameter("@category",filters.Category is not null ? string.Join(',', filters.Category) : ""),
                new SqlParameter("@pageNumber",filters.PageNumber),
                new SqlParameter("@pageSize",filters.PageSize),
                pTotalPages,
                pTotalCount
            };

            var dtResult = await ExecuteQuery(_Sp_Article_GetAllForSearch, arrParams);

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

            return new SQLServerPagedList<Article>(lstArticles, filters.PageNumber.Value, filters.PageSize.Value, totalPages, totalCount);
        }
    }
}
