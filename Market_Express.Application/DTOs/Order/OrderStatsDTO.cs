namespace Market_Express.Application.DTOs.Order
{
    public class OrderStatsDTO
    {
        public int Pending { get; set; }
        public int Finished { get; set; }
        public int Canceled { get; set; }
    }
}
