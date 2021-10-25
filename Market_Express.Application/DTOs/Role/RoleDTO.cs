using Market_Express.Application.DTOs.Permission;
using System;
using System.Collections.Generic;

namespace Market_Express.Application.DTOs.Role
{
    public class RoleDTO
    {
        public RoleDTO()
        {
            Permissions = new();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PermissionDTO> Permissions { get; set; }
    }
}
