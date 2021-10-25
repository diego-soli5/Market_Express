﻿using Market_Express.Application.DTOs.Permission;
using Market_Express.Application.DTOs.Role;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Role
{
    public class RoleEditViewModel
    {
        public RoleEditViewModel()
        {
            Permissions = new();
            PermissionTypes = new();
        }

        public RoleDTO Role { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
        public List<string> PermissionTypes { get; set; }
    }
}
