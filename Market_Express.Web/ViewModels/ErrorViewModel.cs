namespace Market_Express.Web.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {

        }

        public ErrorViewModel(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
