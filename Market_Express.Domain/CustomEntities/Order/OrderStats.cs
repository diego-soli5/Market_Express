namespace Market_Express.Domain.CustomEntities.Order
{
    public class OrderStats
    {
        public OrderStats()
        {
            Pending = 0;
            Finished = 0;
            Canceled = 0;
        }

        public int Pending { get; set; }
        public int Finished { get; set; }
        public int Canceled { get; set; }
    }
}
