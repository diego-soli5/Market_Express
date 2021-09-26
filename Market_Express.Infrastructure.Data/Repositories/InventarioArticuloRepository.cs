using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class InventarioArticuloRepository : GenericRepository<InventarioArticulo>, IInventarioArticuloRepository
    {
        public InventarioArticuloRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context,configuration)
        { }
    }
}
