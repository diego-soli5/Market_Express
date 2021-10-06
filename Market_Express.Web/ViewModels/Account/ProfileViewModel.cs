using Market_Express.Application.DTOs.Address;
using Market_Express.Application.DTOs.AppUser;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Account
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Addresses = new();
        }

        public AppUserProfileDTO AppUser { get; set; }
        public List<AddressDTO> Addresses { get; set; }
    }
}
