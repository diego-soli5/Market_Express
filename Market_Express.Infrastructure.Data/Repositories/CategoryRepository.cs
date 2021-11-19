using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private const string _Sp_Category_GetArticleDetails = "Sp_Category_GetArticleDetails";
        private const string _Sp_Category_GetAllAvailableForSearch = "Sp_Category_GetAllAvailableForSearch";
        private const string _Sp_Category_GetMostPopular = "Sp_Category_GetMostPopular";

        public CategoryRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public IQueryable<Category> GetAllActive()
        {
            return _dbEntity.Where(cat => cat.Status == EntityStatus.ACTIVADO);
        }

        public async Task<List<CategoryForSearch>> GetAllAvailableForSearch()
        {
            List<CategoryForSearch> lstCategoryForSearch = new();

            var dtResult = await ExecuteQuery(_Sp_Category_GetAllAvailableForSearch);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstCategoryForSearch.Add(new CategoryForSearch
                {
                    Id = (Guid)oRow["Id"],
                    Name = oRow["Name"].ToString(),
                    Description = oRow["Description"].ToString(),
                    Status = (EntityStatus)Enum.Parse(typeof(EntityStatus), oRow["Status"].ToString()),
                    Image = oRow["Image"].ToString(),
                    CreationDate = (DateTime)oRow["CreationDate"],
                    ModificationDate = oRow["ModificationDate"] is not DBNull ? (DateTime)oRow["ModificationDate"] : null,
                    AddedBy = oRow["AddedBy"].ToString(),
                    ModifiedBy = oRow["ModifiedBy"].ToString(),
                    ArticlesCount = int.Parse(oRow["ArticlesCount"].ToString()),
                });
            }

            return lstCategoryForSearch;
        }

        public async Task<List<Category>> GetMostPopular(int? take = null)
        {
            List<Category> lstCategory = new();

            var arrParams = new[]
            {
                new SqlParameter("@take",take)
            };

            var dtResult = await ExecuteQuery(_Sp_Category_GetMostPopular,arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstCategory.Add(new CategoryForSearch
                {
                    Id = (Guid)oRow["Id"],
                    Name = oRow["Name"].ToString(),
                    Description = oRow["Description"].ToString(),
                    Image = oRow["Image"].ToString()
                });
            }

            return lstCategory;
        }

        public async Task<(int, int)> GetArticleDetails(Guid categoryId)
        {
            var arrParams = new[]
            {
                new SqlParameter("@CategoryId",categoryId)
            };

            var drResult = (await ExecuteQuery(_Sp_Category_GetArticleDetails, arrParams)).Rows[0];

            int iArticlesEnabledCount = (int)drResult[0];
            int iArticlesDisabledCount = (int)drResult[1];

            return (iArticlesEnabledCount, iArticlesDisabledCount);
        }
    }
}
