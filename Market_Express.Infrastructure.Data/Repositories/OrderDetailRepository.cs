using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    
    }
}
