using AutoMapper;
using Market_Express.Application.DTOs.Address;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class AddressMappings : Profile
    {
        public AddressMappings()
        {
            CreateMap<Address, AddressDTO>()
                .ReverseMap();
        }
    }
}
