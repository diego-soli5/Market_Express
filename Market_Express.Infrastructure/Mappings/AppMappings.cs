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
            CreateMap<InventarioArticulo, ArticuloSyncDTO>()
                .ReverseMap();
        }

        private void CreateClientMappings()
        {
            CreateMap<Cliente, ClienteSyncDTO>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.Usuario.Cedula))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario.Telefono))
                .ForMember(dest => dest.CodCliente, opt => opt.MapFrom(src => src.CodCliente))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }

        private void CreateUsuarioMappings()
        {
            CreateMap<Usuario, LoginRequestDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Clave))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
