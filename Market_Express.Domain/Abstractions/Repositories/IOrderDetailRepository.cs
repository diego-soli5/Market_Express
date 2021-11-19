using Market_Express.Domain.Entities;
using System;
using System.Linq;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        IQueryable<OrderDetail>  GetAllByOrderId(Guid orderId);
    }
}
