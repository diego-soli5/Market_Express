using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Client;
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
        private const string _Sp_Report_GetClientsStatsPaginated = "Sp_Report_GetClientsStatsPaginated";
        private const string _Sp_Report_GetClientsStats = "Sp_Report_GetClientsStats";

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
                new SqlParameter("@minPrice",filters.MinPrice)
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

        public async Task<SQLServerPagedList<ClientForReport>> GetClientsStatsPaginated(ReportClientQueryFilter filters)
        {
            List<ClientForReport> lstArticles = new();

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
                new SqlParameter("@startDate",filters.StartDate),
                new SqlParameter("@endDate",filters.EndDate),
                pTotalPages,
                pTotalCount
            };

            var dtResult = await ExecuteQuery(_Sp_Report_GetClientsStatsPaginated, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ClientForReport
                {
                    AppUser = new()
                    {
                        Name = oRow["Name"].ToString(),
                        Identification = oRow["Identification"].ToString(),
                        Phone = oRow["Phone"].ToString(),
                        Email = oRow["Email"].ToString(),
                    },
                    ClientCode = oRow["ClientCode"].ToString(),
                    Pending = Convert.ToInt32(oRow["Pending"]),
                    Finished = Convert.ToInt32(oRow["Finished"]),
                    Canceled = Convert.ToInt32(oRow["Canceled"]),
                });
            }

            return new SQLServerPagedList<ClientForReport>(lstArticles, filters.PageNumber.Value, filters.PageSize.Value, Convert.ToInt32(pTotalPages.Value), Convert.ToInt32(pTotalCount.Value));
        }

        public async Task<List<ClientForReport>> GetClientsStats(ReportClientQueryFilter filters)
        {
            List<ClientForReport> lstArticles = new();

            var arrParams = new[]
            {
                new SqlParameter("@startDate",filters.StartDate),
                new SqlParameter("@endDate",filters.EndDate)
            };

            var dtResult = await ExecuteQuery(_Sp_Report_GetClientsStats, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstArticles.Add(new ClientForReport
                {
                    AppUser = new()
                    {
                        Name = oRow["Name"].ToString(),
                        Identification = oRow["Identification"].ToString(),
                        Phone = oRow["Phone"].ToString(),
                        Email = oRow["Email"].ToString(),
                    },
                    ClientCode = oRow["ClientCode"].ToString(),
                    Pending = Convert.ToInt32(oRow["Pending"]),
                    Finished = Convert.ToInt32(oRow["Finished"]),
                    Canceled = Convert.ToInt32(oRow["Canceled"]),
                });
            }

            return lstArticles;
        }
    }
}
