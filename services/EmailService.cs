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
            _config = configuration.GetSection("EmailConfig").Get<EmailConfig>() ?? throw new ArgumentNullException("EmailConfig", "Email configuration is missing in the settings.");
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
        public void ConfirmEmailToSendOnSetup()
        {
            try
            {
                SendEmail("Stock Quote Service Setup Confirmation", "The Stock Quote Service has been set up successfully.");
                Console.WriteLine("Confirmation email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
            }
        }
        public void PurchaseNotification(string tickerSymbol, decimal purchasePoint, StockQuotation quotation)
        {
            string subject = $"Purchase Notification for {tickerSymbol}";
            string body = $"The stock {tickerSymbol} has reached the purchase point of {purchasePoint}.\n" +
                          $"Current price: {quotation.Price} at {quotation.Timestamp}";

            SendEmail(subject, body);
        }
        public void SaleNotification(string tickerSymbol, decimal salePoint, StockQuotation quotation)
        {
            string subject = $"Sale Notification for {tickerSymbol}";
            string body = $"The stock {tickerSymbol} has reached the sale point of {salePoint}.\n" +
                          $"Current price: {quotation.Price} at {quotation.Timestamp}";

            SendEmail(subject, body);
        }
    }
}