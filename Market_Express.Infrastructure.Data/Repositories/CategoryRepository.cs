using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private const string _Sp_Category_GetArticleDetails = "Sp_Category_GetArticleDetails";

        public CategoryRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context,configuration)
        { }

        public async Task<(int,int)> GetArticleDetails(Guid categoryId)
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
