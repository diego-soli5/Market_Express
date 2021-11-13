using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    

        public IEnumerable<OrderDetail> GetAllByOrderId(Guid orderId)
        {
            return _dbEntity.Where(od => od.OrderId == orderId).AsEnumerable();
        }

    }
}
