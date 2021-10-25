using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Role
{
    public class RoleCreateDTO
    {
        public RoleCreateDTO()
        {
            Permissions = new();
        }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(255)]
        public string Description { get; set; }

        public List<Guid> Permissions { get; set; }
    }
}
