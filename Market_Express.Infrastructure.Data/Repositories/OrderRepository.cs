using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private const string _Sp_Order_GetStatsByUserId = "Sp_Order_GetStatsByUserId";
        private const string _Sp_Order_GetStats = "Sp_Order_GetStats";
        private const string _Sp_Order_GetMostRecentByUserId = "Sp_Order_GetMostRecentByUserId";
        private const string _Sp_Order_GetDetailsById = "Sp_Order_GetDetailsById";
        private const string _Sp_Order_GetMostRecent = "Sp_Order_GetMostRecent";

        public OrderRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public IEnumerable<Order> GetAllByUserId(Guid userId)
        {
            return _dbEntity.Where(o => o.Client.AppUserId == userId).AsEnumerable();
        }

        public async Task<OrderStats> GetStats()
        {
            OrderStats oOrderStats = new();

            var dtResult = await ExecuteQuery(_Sp_Order_GetStats);

            if (dtResult?.Rows?.Count > 0)
            {
                var drResult = dtResult.Rows[0];

                oOrderStats.Pending = int.Parse(drResult[0].ToString());
                oOrderStats.Finished = int.Parse(drResult[1].ToString());
                oOrderStats.Canceled = int.Parse(drResult[2].ToString());
            }

            return oOrderStats;
        }

        public async Task<OrderStats> GetStatsByUserId(Guid userId)
        {
            OrderStats oOrderStats = new();

            var arrParams = new[]
            {
                new SqlParameter("@userId",userId)
            };

            var dtResult = await ExecuteQuery(_Sp_Order_GetStatsByUserId, arrParams);

            if (dtResult?.Rows?.Count > 0)
            {
                var drResult = dtResult.Rows[0];

                oOrderStats.Pending = int.Parse(drResult[0].ToString());
                oOrderStats.Finished = int.Parse(drResult[1].ToString());
                oOrderStats.Canceled = int.Parse(drResult[2].ToString());
            }

            return oOrderStats;
        }

        public async Task<List<RecentOrder>> GetMostRecentByUserId(Guid userId, int? take = null)
        {
            List<RecentOrder> lstRecentOrders = new();

            var arrParams = new[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@take",take)
            };

            var dtResult = await ExecuteQuery(_Sp_Order_GetMostRecentByUserId, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstRecentOrders.Add(new RecentOrder
                {
                    Id = (Guid)oRow["Id"],
                    CreationDate = (DateTime)oRow["CreationDate"],
                    OrderNumber = int.Parse(oRow["OrderNumber"].ToString()),
                    Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), oRow["Status"].ToString()),
                    MostRequestedArticleImage = oRow["MostRequestedArticleImage"] is DBNull ? null : oRow["MostRequestedArticleImage"].ToString()
                });
            }

            return lstRecentOrders;
        }

        public async Task<List<RecentOrder>> GetMostRecent(int? take = null, bool onlyPending = false)
        {
            List<RecentOrder> lstRecentOrders = new();

            var arrParams = new[]
            {
                new SqlParameter("@onlyPending",onlyPending),
                new SqlParameter("@take",take)
            };

            var dtResult = await ExecuteQuery(_Sp_Order_GetMostRecent, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstRecentOrders.Add(new RecentOrder
                {
                    Id = (Guid)oRow["Id"],
                    CreationDate = (DateTime)oRow["CreationDate"],
                    OrderNumber = int.Parse(oRow["OrderNumber"].ToString()),
                    Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), oRow["Status"].ToString()),
                    MostRequestedArticleImage = oRow["MostRequestedArticleImage"] is DBNull ? null : oRow["MostRequestedArticleImage"].ToString()
                });
            }

            return lstRecentOrders;
        }

        public async Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid orderId)
        {
            List<OrderArticleDetail> lstOrderDetails = new();

            var arrParams = new[]
            {
                new SqlParameter("@orderId",orderId),
            };

            var dtResult = await ExecuteQuery(_Sp_Order_GetDetailsById, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstOrderDetails.Add(new OrderArticleDetail
                {
                    Id = (Guid)oRow["Id"],
                    Description = oRow["Description"].ToString(),
                    BarCode = oRow["BarCode"].ToString(),
                    Quantity = (decimal)oRow["Quantity"],
                    Price = (decimal)oRow["Price"]
                });
            }

            return lstOrderDetails;
        }

        public IEnumerable<Order> GetAllIncludeAppUser()
        {
            return _dbEntity.Include(o => o.Client)
                            .ThenInclude(c => c.AppUser)
                            .AsEnumerable();
        }
    }
}
