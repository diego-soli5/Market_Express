using AutoMapper;
using Market_Express.Application.DTOs.Permission;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class PermissionMappings : Profile
    {
        public PermissionMappings()
        {
            CreateMap<Permission, PermissionDTO>()
                .ReverseMap();
        }
    }
}
