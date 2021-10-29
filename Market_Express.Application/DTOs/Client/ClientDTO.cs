using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Client
{
    public class ClientDTO
    {
        public string ClientCode { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "AppUser.Client.AutoSync")]
        public bool AutoSync { get; set; }
    }
}
