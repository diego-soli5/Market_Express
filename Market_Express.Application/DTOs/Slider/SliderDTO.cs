using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Slider
{
    public class SliderDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public EntityStatus Status { get; set; }
    }
}
