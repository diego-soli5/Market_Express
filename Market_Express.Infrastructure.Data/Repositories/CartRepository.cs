using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private const string _Sp_Cart_GetArticlesCount = "Sp_Cart_GetArticlesCount";

        public CartRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<int> GetArticlesCount(Guid userId)
        {
            int iCount = 0;

            var arrParams = new[]
            {
                new SqlParameter("@UserId",userId)
            };

            var dtResult = await ExecuteQuery(_Sp_Cart_GetArticlesCount, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                iCount = int.Parse(oRow[0].ToString());
            }

            return iCount;
        }
    }
}
