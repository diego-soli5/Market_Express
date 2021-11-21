using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private const string _Sp_Cart_GetArticlesCount = "Sp_Cart_GetArticlesCount";
        private const string _Sp_Cart_GetOpenCountByArticleId = "Sp_Cart_GetOpenCountByArticleId";
        private const string _Sp_Cart_GetCurrentByUserId = "Sp_Cart_GetCurrentByUserId";

        public CartRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
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
            Cart oCart = null;

            var arrParams = new[]
            {
                new SqlParameter("@userId",userId)
            };

            var dtResult = await ExecuteQuery(_Sp_Cart_GetCurrentByUserId, arrParams);

            if (dtResult?.Rows?.Count > 0)
            {
                var drResult = dtResult.Rows[0];

                oCart = new();
                oCart.Id = (Guid)drResult["Id"];
                oCart.ClientId = (Guid)drResult["ClientId"];
                oCart.OpeningDate = Convert.ToDateTime(drResult["OpeningDate"]);
                oCart.Status = (CartStatus)Enum.Parse(typeof(CartStatus), drResult["Status"].ToString());
            }

            return oCart;
        }
    }
}
