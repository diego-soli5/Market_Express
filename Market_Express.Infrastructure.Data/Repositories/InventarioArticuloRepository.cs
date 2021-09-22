using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class InventarioArticuloRepository : GenericRepository<InventarioArticulo>, IInventarioArticuloRepository
    {
        public InventarioArticuloRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context,configuration)
        { }
    }
}
