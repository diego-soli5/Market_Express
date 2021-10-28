using Market_Express.Application.DTOs.AppUser;
using Market_Express.Domain.QueryFilter.AppUser;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Users
{
    public class UserIndexViewModel
    {
        public List<AppUserDTO> AppUsers { get; set; }
        public AppUserIndexQueryFilter QueryFilter { get; set; }
    }
}
