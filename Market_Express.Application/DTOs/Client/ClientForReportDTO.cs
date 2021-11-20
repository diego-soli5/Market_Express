using Market_Express.Application.DTOs.AppUser;

namespace Market_Express.Application.DTOs.Client
{
    public class ClientForReportDTO
    {
        public string ClientCode { get; set; }
        public int Pending { get; set; }
        public int Finished { get; set; }
        public int Canceled { get; set; }

        public int TotalOrders => Pending + Finished + Canceled;

        public AppUserDTO AppUser { get; set; }
    }
}
