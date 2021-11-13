using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        IEnumerable<OrderDetail>  GetAllByOrderId(Guid orderId);
    }
}
