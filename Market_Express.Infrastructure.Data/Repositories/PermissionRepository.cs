using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    
    }
}
