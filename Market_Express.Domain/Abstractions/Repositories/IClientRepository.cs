using Market_Express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetByUserIdAsync(Guid userId);
    }
}
