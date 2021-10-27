using Market_Express.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.AppUser
{
    public class AppUserCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(40)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Identification { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresa una dirección correo electronico válida.")]
        [StringLength(40)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(40)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public AppUserType Type { get; set; }


    }
}
