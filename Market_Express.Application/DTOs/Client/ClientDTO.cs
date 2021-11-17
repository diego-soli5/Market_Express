using Market_Express.Application.DTOs.AppUser;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Market_Express.Application.DTOs.Client
{
    public class ClientDTO
    {
        public Guid Id { get; set; }

        [BindProperty(Name = "Client.ClientCode")]
        public string ClientCode { get; set; }

        [BindProperty(Name = "Client.AutoSync")]
        public bool AutoSync { get; set; }

        public AppUserDTO AppUser { get; set; }
    }
}
