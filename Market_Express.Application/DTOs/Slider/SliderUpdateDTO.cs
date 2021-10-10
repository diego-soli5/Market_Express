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
        public string Status { get; set; }
        public IFormFile Image { get; set; }
    }
}
