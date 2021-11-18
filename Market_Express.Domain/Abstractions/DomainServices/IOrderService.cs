using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IOrderService
    {
        Task<BusisnessResult> Generate(Guid userId);
        Task<Order> GetById(Guid id, Guid? currentUserId = null, bool includeAppUser = false);
        Task<List<RecentOrder>> GetMostRecent();
        Task<BusisnessResult> SetFinished(Guid orderId);
        PagedList<Order> GetAllPaginatedByUserId(Guid userId, MyOrdersQueryFilter filters);
        PagedList<Order> GetAllPaginated(AdminOrderQueryFilter filters);
        Task<List<RecentOrder>> GetMostRecentByUserId(Guid userId);
        Task<OrderStats> GetOrderStatsByUserId(Guid userId);
        Task<OrderStats> GetOrderStats();
        Task<BusisnessResult> CancelMostRecent(Guid userId);
        Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid id);
    }
}
