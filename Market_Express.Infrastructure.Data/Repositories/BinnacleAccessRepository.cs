using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class BinnacleAccessRepository : GenericRepository<BinnacleAccess>, IBinnacleAccessRepository
    {
        public BinnacleAccessRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    
        public async Task<BinnacleAccess> GetLastAccessByUserId(Guid userId)
        {
            return await _dbEntity.Where(a => a.ExitDate == null && a.AppUserId == userId)
                                  .OrderByDescending(a => a.EntryDate)
                                  .FirstOrDefaultAsync();
        }
    }
}
