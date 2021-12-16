using AutoMapper;
using Market_Express.Application.DTOs.Account;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class AppUserMappings : Profile
    {
        public AppUserMappings()
        {
            CreateMap<AppUser, LoginRequestDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<AppUser, AppUserProfileDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserEditDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserDetailsDTO>()
                .ReverseMap();
        }
    }
}
