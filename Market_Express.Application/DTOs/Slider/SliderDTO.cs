using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Slider
{
    public class SliderDTO
    {
        public SliderDTO()
        {

        }

        public SliderDTO(string name)
        {
            Name = name;
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Image { get; set; }
        public string Status { get; set; }
    }
}
