namespace Market_Express.CrossCutting.Options
{
    public class EmailServicesOptions
    {
        public string SenderMail { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
    }
}
