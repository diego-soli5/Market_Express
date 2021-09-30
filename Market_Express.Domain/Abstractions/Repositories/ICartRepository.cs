using Market_Express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<int> GetArticlesCount(Guid userId);
    }
}
