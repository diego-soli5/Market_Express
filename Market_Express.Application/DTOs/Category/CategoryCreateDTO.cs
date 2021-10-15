using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public IFormFile Image { get; set; }
    }
}
