using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        IEnumerable<Address> GetAllByUserId(Guid id);
    }
}
