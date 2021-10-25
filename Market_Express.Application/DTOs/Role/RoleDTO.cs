using Market_Express.Application.DTOs.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Role
{
    public class RoleDTO
    {
        public RoleDTO()
        {
            Permissions = new();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(255)]
        public string Description { get; set; }

        public List<PermissionDTO> Permissions { get; set; }
    }
}
