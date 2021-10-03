﻿using Market_Express.CrossCutting.Options;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Market_Express.Infrastructure.EmailServices
{
    public abstract class MasterMailServer
    {
        private readonly EmailServicesOptions _options;

        public MasterMailServer(IOptions<EmailServicesOptions> options)
        {
            _options = options.Value;
        }

        public void SendMail(string subject, string body, string receiverMail)
        {
            var receiversMails = new List<string>
            {
                receiverMail
            };

            SendMail(subject, body, receiversMails);
        }

        public void SendMail(string subject, string body, List<string> receiversMails)
        {
            using (var smtpClient = new SmtpClient())
            {
                InitializeSmtpClient(smtpClient);

                using (var mailMessage = new MailMessage())
                { 
                    mailMessage.From = new MailAddress(_options.SenderMail);

                    foreach (string mail in receiversMails)
                    {
                        mailMessage.To.Add(mail);
                    }

                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.Priority = MailPriority.Normal;

                    smtpClient.Send(mailMessage);
                }
            }
        }

        private void InitializeSmtpClient (SmtpClient client)
        {
            client.Credentials = new NetworkCredential(_options.SenderMail, _options.Password);
            client.Host = _options.Host;
            client.Port = _options.Port;
            client.EnableSsl = _options.Ssl;
        }
    }
}