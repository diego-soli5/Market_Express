using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private const string _Sp_Cart_GetArticlesCount = "Sp_Cart_GetArticlesCount";
        private const string _Sp_Cart_GetOpenCountByArticleId = "Sp_Cart_GetOpenCountByArticleId";

        public CartRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<int> GetArticlesCount(Guid userId)
        {
            int iCount;

            var arrParams = new[]
            {
                new SqlParameter("@UserId",userId)
            };

            var dtResult = await ExecuteQuery(_Sp_Cart_GetArticlesCount, arrParams);

            var drResult = dtResult.Rows[0];

            iCount = int.Parse(drResult[0].ToString());

            return iCount;
        }

        public async Task<int> GetOpenCountByArticleId(Guid articleId)
        {
            int iCount;

            var arrParams = new[]
            {
                new SqlParameter("@articleId",articleId)
            };

            var dtResult = await ExecuteQuery(_Sp_Cart_GetOpenCountByArticleId, arrParams);

            var drResult = dtResult.Rows[0];

            iCount = int.Parse(drResult[0].ToString());
            
            return iCount;
        }

        public async Task<Cart> GetCurrentByUserId(Guid userId)
        {
            return await _dbEntity.Where(c => c.Client.AppUserId == userId &&
                                              c.Status == CartStatus.ABIERTO)
                                  .FirstOrDefaultAsync();
        }
    }
}
