using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.System
{
    public class ClienteSyncDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ClientCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Identification { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
