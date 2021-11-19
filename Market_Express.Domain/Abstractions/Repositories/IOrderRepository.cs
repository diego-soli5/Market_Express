using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<OrderStats> GetStatsByUserId(Guid userId);
        Task<OrderStats> GetStats();
        Task<List<RecentOrder>> GetMostRecentByUserId(Guid userId, int? take = null);
        Task<List<RecentOrder>> GetMostRecent(int? take = null, bool onlyPending = false);
        IQueryable<Order> GetAllByUserId(Guid userId);
        Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid orderId);
        IQueryable<Order> GetAllIncludeAppUser();
        Task<Order> GetByIdIncludeAppUserAsync(Guid id);
    }
}
