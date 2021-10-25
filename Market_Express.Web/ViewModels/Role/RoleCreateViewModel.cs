using Market_Express.Application.DTOs.Permission;
using Market_Express.Application.DTOs.Role;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Role
{
    public class RoleCreateViewModel
    {
        public RoleCreateViewModel()
        {
            PermissionsAvailable = new();
            PermissionTypes = new();
            Role = new();
        }

        public RoleCreateDTO Role { get; set; }
        public List<PermissionDTO> PermissionsAvailable { get; set; }
        public List<string> PermissionTypes { get; set; }
    }
}
