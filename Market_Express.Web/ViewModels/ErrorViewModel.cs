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

        public ErrorViewModel(string requestId, string message)
        {
            RequestId = requestId;
            Message = message;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; }
    }
}
