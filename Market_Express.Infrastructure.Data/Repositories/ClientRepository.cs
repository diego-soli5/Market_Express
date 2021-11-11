using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<Client> GetByUserIdAsync(Guid userId)
        {
            return await _dbEntity.FirstOrDefaultAsync(c => c.AppUserId == userId);
        }
    }
}
