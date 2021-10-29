using Market_Express.Application.DTOs.AppUser;
using Market_Express.Application.DTOs.Role;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Users
{
    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
            AppUser = new();
            AvailableRoles = new();
        }

        public AppUserEditDTO AppUser { get; set; }
        public List<RoleDTO> AvailableRoles { get; set; }
    }
}
