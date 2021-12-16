using AutoMapper;
using Market_Express.Application.DTOs.Role;
using Market_Express.Domain.CustomEntities.Role;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class RoleMappings : Profile
    {
        public RoleMappings()
        {
            CreateMap<Role, RoleDTO>()
                .ReverseMap();

            CreateMap<RoleWithPermissions, RoleDTO>()
                .ReverseMap();
        }
    }
}
