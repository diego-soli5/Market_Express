using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Report;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class ReportRepository : ADORepository, IReportRepository
    {
        private const string _Sp_Report_GetMostSoldArticlesPaginated = "Sp_Report_GetMostSoldArticlesPaginated";
        private const string _Sp_Report_GetMostSoldArticles = "Sp_Report_GetMostSoldArticles";

        public ReportRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<SQLServerPagedList<ArticleForReport>> GetMostSoldArticlesPaginated(ReportArticleQueryFilter filters)
        {
            List<ArticleForReport> lstArticles = new();

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
                new SqlParameter("@categoryId",filters.CategoryId),
                new SqlParameter("@description",filters.Description),
                new SqlParameter("@maxPrice",filters.MaxPrice),
                new SqlParameter("@minPrice",filters.MinPrice),
                new SqlParameter("@pageNumber",filters.PageNumber-1),
                new SqlParameter("@pageSize",filters.PageSize),
                pTotalPages,
                pTotalCount
            };

            var dtResult = await ExecuteQuery(_Sp_Report_GetMostSoldArticlesPaginated, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ArticleForReport
                {
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Price = Convert.ToDecimal(oRow["Price"]),
                    SoldUnitsCount = Convert.ToInt32(oRow["SoldUnitsCount"]),
                    Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), oRow["Status"].ToString()),
                    Category = new Category
                    {
                        Name = oRow["CategoryName"] is DBNull ? null : oRow["CategoryName"].ToString()
                    }
                });
            }

            return new SQLServerPagedList<ArticleForReport>(lstArticles, filters.PageNumber.Value, filters.PageSize.Value, Convert.ToInt32(pTotalPages.Value), Convert.ToInt32(pTotalCount.Value));
        }

        public async Task<List<ArticleForReport>> GetMostSoldArticles(ReportArticleQueryFilter filters)
        {
            List<ArticleForReport> lstArticles = new();

            var arrParams = new[]
            {
                new SqlParameter("@categoryId",filters.CategoryId),
                new SqlParameter("@description",filters.Description),
                new SqlParameter("@maxPrice",filters.MaxPrice),
                new SqlParameter("@minPrice",filters.MinPrice),
            };

            var dtResult = await ExecuteQuery(_Sp_Report_GetMostSoldArticles, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ArticleForReport
                {
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Price = Convert.ToDecimal(oRow["Price"]),
                    SoldUnitsCount = Convert.ToInt32(oRow["SoldUnitsCount"]),
                    Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), oRow["Status"].ToString()),
                    Category = new Category
                    {
                        Name = oRow["CategoryName"] is DBNull ? null : oRow["CategoryName"].ToString()
                    }
                });
            }

            return lstArticles;
        }
    }
}
