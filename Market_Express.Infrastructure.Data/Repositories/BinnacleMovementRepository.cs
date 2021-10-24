using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class BinnacleMovementRepository : GenericRepository<BinnacleAccess>, IBinnacleMovementRepository
    {
        public BinnacleMovementRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    }
}
