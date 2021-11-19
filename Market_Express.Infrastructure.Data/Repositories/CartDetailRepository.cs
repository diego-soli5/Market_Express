using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class CartDetailRepository : GenericRepository<CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }
    
        public IQueryable<CartDetail> GetAllByCartId(Guid id)
        {
            return _dbEntity.Where(cd => cd.CartId == id);
        }

    }
}
