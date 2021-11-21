using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
        { }


        public IQueryable<OrderDetail> GetAllByOrderId(Guid orderId)
        {
            return _dbEntity.Where(od => od.OrderId == orderId);
        }

    }
}
