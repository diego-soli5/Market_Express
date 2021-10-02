using System.ComponentModel.DataAnnotations;

namespace Market_Express.Web.ViewModels.Account
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Es requerido")]
        [StringLength(10,ErrorMessage = "Minumo 10")]
        [Compare(nameof(Alias),ErrorMessage = "Dee ser igualñ")]
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
