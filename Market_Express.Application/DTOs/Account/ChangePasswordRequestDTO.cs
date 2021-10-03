namespace Market_Express.Application.DTOs.Account
{
    public class ChangePasswordRequestDTO
    {
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }
        public string NewPassConfirmation { get; set; }
    }
}
