using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.AppUser
{
    public class AppUserEditDTO
    {
        public AppUserEditDTO()
        {
            Roles = new();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(40)]
        [BindProperty(Name = "AppUser.Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "AppUser.Identification")]
        public string Identification { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresa una dirección correo electrónico válida.")]
        [StringLength(40)]
        [BindProperty(Name = "AppUser.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(8)]
        [BindProperty(Name = "AppUser.Phone")]
        [Phone(ErrorMessage = "Ingresa un número de teléfono válido.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "AppUser.Type")]
        public AppUserType Type { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "AppUser.Status")]
        public EntityStatus Status { get; set; }

        public List<Guid> Roles { get; set; }
    }
}
