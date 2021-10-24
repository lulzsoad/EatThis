using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IEmailService
    {
        Task SendByNoReply(string destinationEmailAddress, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private SmtpClient smtpClient;
        private MailMessage mailMessage;
        private readonly NoReplyEmailSettings noReplyEmailSettings;

        public EmailService(NoReplyEmailSettings emailSettings)
        {
            this.noReplyEmailSettings = emailSettings;
        }

        public async Task SendByNoReply(string destinationEmailAddress, string subject, string body)
        {
            mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(noReplyEmailSettings.SenderEmail, noReplyEmailSettings.SenderName);
            mailMessage.To.Add(new MailAddress(destinationEmailAddress));
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = body;

            smtpClient = new SmtpClient
            {
                Host = noReplyEmailSettings.HostSmtp,
                EnableSsl = noReplyEmailSettings.EnableSsl,
                Port = noReplyEmailSettings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(noReplyEmailSettings.SenderEmail, noReplyEmailSettings.SenderEmailPassword)
            };

            smtpClient.SendCompleted += OnSendCompleted;

            await smtpClient.SendMailAsync(mailMessage);
        }

        private void OnSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            smtpClient.Dispose();
            mailMessage.Dispose();
        }
    }
}
