namespace Market_Express.Application.DTOs.Account
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool FromModal { get; set; }
    }
}
