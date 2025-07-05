using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Quotation.Models;

namespace Quotation.Services
{
    public class EmailService
    {
        private readonly EmailConfig Config;

        public EmailService(IConfiguration Configuration)
        {
            Config = Configuration.GetSection("EmailConfig").Get<EmailConfig>() ?? throw new ArgumentNullException("EmailConfig", "Email configuration is missing in the settings.");
        }

        public void SendEmail(string Subject, string Body)
        {
            using var Client = new SmtpClient(Config.SmtpServer, Config.SmtpPort)
            {
                Credentials = new NetworkCredential(Config.SmtpUser, Config.SmtpPassword),
                EnableSsl = true
            };

            var Mail = new MailMessage(Config.From, Config.To, Subject, Body);
            Client.Send(Mail);
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
        public void PurchaseNotification(string TickerSymbol, decimal PurchasePoint, StockQuotation Quotation)
        {
            string Subject = $"Purchase Notification for {TickerSymbol}";
            string Body = $"The stock {TickerSymbol} has reached the purchase point of {PurchasePoint}.\n" +
                          $"Current price: {Quotation.Price} at {Quotation.Timestamp}";

            SendEmail(Subject, Body);
        }
        public void SaleNotification(string TickerSymbol, decimal SalePoint, StockQuotation Quotation)
        {
            string Subject = $"Sale Notification for {TickerSymbol}";
            string Body = $"The stock {TickerSymbol} has reached the sale point of {SalePoint}.\n" +
                          $"Current price: {Quotation.Price} at {Quotation.Timestamp}";

            SendEmail(Subject, Body);
        }
    }
}