using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ICartDetailRepository : IGenericRepository<CartDetail>
    {
        IEnumerable<CartDetail> GetAllByCartId(Guid id);
    }
}
