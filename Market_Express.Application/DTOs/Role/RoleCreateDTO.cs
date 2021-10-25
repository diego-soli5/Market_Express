using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Role
{
    public class RoleCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [StringLength(255)]
        public string Description { get; set; }
    }
}
