using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Quotation.Models;

namespace Quotation.Services
{
    public class EmailService
    {
        private readonly EmailConfig _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration.GetSection("EmailConfig").Get<EmailConfig>();
        }

        public void SendEmail(string subject, string body)
        {
            using var client = new SmtpClient(_config.SmtpServer, _config.SmtpPort)
            {
                Credentials = new NetworkCredential(_config.SmtpUser, _config.SmtpPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(_config.From, _config.To, subject, body);
            client.Send(mail);
        }
    }
}