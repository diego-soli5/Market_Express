using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAddressService
    {
        Task<BusisnessResult> Edit(Address address, Guid userId);
        Task<BusisnessResult> Create(Address address, Guid userId);
        Task<IEnumerable<Address>> GetAllByUserId(Guid userId);
        Task<Address> GetById(Guid addressId);
        Task<BusisnessResult> SetForUse(Guid addressId, Guid userId);
        Task<BusisnessResult> CreateFromCart(Address address, Guid userId);
    }
}
