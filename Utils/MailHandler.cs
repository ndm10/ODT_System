using Microsoft.Extensions.Options;
using ODT_System.SharedObject;
using System.Net.Mail;
using System.Net;
using ODT_System.Utils.Interface;

namespace ODT_System.Utils
{
    public class MailHandler : IMailHandler
    {
        private readonly AppSettings _appSettings;

        public MailHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(string recipient, string subject, string body)
        {
            var sender = _appSettings.EmailSender;
            var senderPass = _appSettings.EmailSenderPassword;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(sender, senderPass)
            };

            MailMessage mailMessage = new MailMessage(sender, recipient, subject, body)
            {
                IsBodyHtml = true
            };

            smtpClient.Send(mailMessage);
        }
    }
}
