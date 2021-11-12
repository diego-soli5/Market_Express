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
        Task<Order> GetById(Guid id);
        PagedList<Order> GetAllByUserId(Guid userId, MyOrdersQueryFilter filters);
        Task<List<RecentOrder>> GetRecentOrdersByUserId(Guid userId);
        Task<OrderStats> GetOrderStatsByUserId(Guid userId);
        Task<BusisnessResult> CancelMostRecent(Guid userId);
        Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid id);
    }
}
