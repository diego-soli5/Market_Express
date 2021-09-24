namespace Market_Express.Domain
{
    public class BusisnessResult
    {
        public BusisnessResult()
        {
            Success = false;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
