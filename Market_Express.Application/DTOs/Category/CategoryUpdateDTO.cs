using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public EntityStatus Status { get; set; }

        public string Image { get; set; }

        public IFormFile NewImage { get; set; }
    }
}
