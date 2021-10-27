using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.AppUser
{
    public class AppUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public EntityStatus Status { get; set; }
    }
}
