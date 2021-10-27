using Market_Express.Application.DTOs.Permission;
using Market_Express.Application.DTOs.Role;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Role
{
    public class RoleDetailsViewModel
    {
        public RoleDetailsViewModel()
        {
            PermissionsAvailable = new();
            Role = new();
        }

        public RoleDTO Role { get; set; }
        public List<PermissionDTO> PermissionsAvailable { get; set; }
        public List<string> PermissionTypes { get; set; }

        public int ActiveUsersUsingThisRole { get; set; }
        public int DisabledUsersUsingThisRole { get; set; }

        public int PermissionsCount => Role.Permissions.Count;
        public int UsersUsingThisRoleCount => ActiveUsersUsingThisRole + DisabledUsersUsingThisRole;
    }
}
