namespace Market_Express.Domain.CustomEntities.Client
{
    public class ClientForReport : Entities.Client
    {
        public int Pending { get; set; }
        public int Finished { get; set; }
        public int Canceled { get; set; }
    }
}
