using Market_Express.Application.DTOs.Role;
using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace Market_Express.Application.DTOs.AppUser
{
    public class AppUserDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AppUserType Type { get; set; }
        public EntityStatus Status { get; set; }

        public bool IsInPOS { get; set; }

        public List<RoleDTO> Roles { get; set; }
    }
}
