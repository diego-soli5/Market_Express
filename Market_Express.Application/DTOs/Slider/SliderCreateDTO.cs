using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Slider
{
    public class SliderCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public IFormFile Image { get; set; }
    }
}
