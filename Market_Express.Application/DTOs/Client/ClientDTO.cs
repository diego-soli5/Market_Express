using Microsoft.AspNetCore.Mvc;

namespace Market_Express.Application.DTOs.Client
{
    public class ClientDTO
    {
        [BindProperty(Name = "Client.ClientCode")]
        public string ClientCode { get; set; }

        [BindProperty(Name = "Client.AutoSync")]
        public bool AutoSync { get; set; }
    }
}
