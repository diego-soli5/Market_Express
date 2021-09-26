using AutoMapper;
using Market_Express.Application.DTOs.Access;
using Market_Express.Application.DTOs.System;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class AppMappings : Profile
    {
        public AppMappings()
        {
            CreateArticuloMappings();
            CreateClientMappings();
            CreateUsuarioMappings();
        }

        private void CreateArticuloMappings()
        {
            CreateMap<Article, ArticuloSyncDTO>()
                .ReverseMap();
        }

        private void CreateClientMappings()
        {
            CreateMap<Client, ClienteSyncDTO>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.AppUser.Identification))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.AppUser.Phone))
                .ForMember(dest => dest.CodCliente, opt => opt.MapFrom(src => src.ClientCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }

        private void CreateUsuarioMappings()
        {
            CreateMap<AppUser, LoginRequestDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
