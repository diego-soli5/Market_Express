using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;


namespace Market_Express.Application.DTOs.Slider
{
    public class SliderUpdateDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public EntityStatus Status { get; set; }

        public string Image { get; set; }

        public IFormFile NewImage { get; set; }
    }
}
