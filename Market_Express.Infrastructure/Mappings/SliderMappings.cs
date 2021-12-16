using AutoMapper;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class SliderMappings : Profile
    {
        public SliderMappings()
        {
            CreateMap<Slider, SliderDTO>()
                .ReverseMap();

            CreateMap<Slider, SliderCreateDTO>()
                .ReverseMap();

            CreateMap<Slider, SliderUpdateDTO>()
                .ReverseMap();
        }
    }
}
