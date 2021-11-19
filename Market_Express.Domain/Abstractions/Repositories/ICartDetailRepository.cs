using Market_Express.Domain.Entities;
using System;
using System.Linq;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ICartDetailRepository : IGenericRepository<CartDetail>
    {
        IQueryable<CartDetail> GetAllByCartId(Guid id);
    }
}
