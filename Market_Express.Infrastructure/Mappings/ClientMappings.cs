using AutoMapper;
using Market_Express.Application.DTOs.Client;
using Market_Express.Application.DTOs.System;
using Market_Express.Domain.CustomEntities.Client;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class ClientMappings : Profile
    {
        public ClientMappings()
        {
            CreateMap<Client, ClientDTO>()
                .ReverseMap();

            CreateMap<ClientForReport, ClientForReportDTO>()
                .ForPath(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser))
                .ReverseMap();

            CreateMap<Client, ClienteSyncDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.AppUser.Identification))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.Phone))
                .ForMember(dest => dest.ClientCode, opt => opt.MapFrom(src => src.ClientCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
