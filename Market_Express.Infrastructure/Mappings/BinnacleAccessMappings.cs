using AutoMapper;
using Market_Express.Application.DTOs.BinnacleAccess;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class BinnacleAccessMappings : Profile
    {
        public BinnacleAccessMappings()
        {
            CreateMap<BinnacleAccess, BinnacleAccessDTO>()
                .ReverseMap();
        }
    }
}
