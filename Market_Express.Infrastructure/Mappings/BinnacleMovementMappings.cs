using AutoMapper;
using Market_Express.Application.DTOs.BinnacleMovement;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class BinnacleMovementMappings : Profile
    {
        public BinnacleMovementMappings()
        {
            CreateMap<BinnacleMovement, BinnacleMovementDTO>()
                .ReverseMap();
        }
    }
}
