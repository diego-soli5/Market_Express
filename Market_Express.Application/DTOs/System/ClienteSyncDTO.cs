using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.System
{
    public class ClienteSyncDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string CodCliente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefono { get; set; }
    }
}
