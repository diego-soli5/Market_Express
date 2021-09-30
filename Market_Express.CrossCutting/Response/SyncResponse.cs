namespace Market_Express.CrossCutting.Response
{
    public class SyncResponse
    {
        public SyncResponse()
        {
            AddedCount = 0;
            UpdatedCount = 0;
        }

        public int AddedCount { get; set; }
        public int UpdatedCount { get; set; }
        public bool Success { get; set; }
    }
}
