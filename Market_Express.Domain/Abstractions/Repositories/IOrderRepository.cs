using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<OrderStats> GetStatsByUserId(Guid userId);
        Task<List<RecentOrder>> GetMostRecentByUserId(Guid userId, int? take = null);
        IEnumerable<Order> GetAllByUserId(Guid userId);
        Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid orderId);
    }
}
