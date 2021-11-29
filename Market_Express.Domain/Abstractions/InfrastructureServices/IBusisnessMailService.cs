using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.InfrastructureServices
{
    public interface IBusisnessMailService
    {
        void SendMail(string subject, string body, string[] receiversMails);
        void SendMail(string subject, string body, List<string> receiversMails);
        void SendMail(string subject, string body, string receiverMail);
    }
}
