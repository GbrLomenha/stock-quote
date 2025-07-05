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

        public async Task SendEmail(string Subject, string Body)
        {
            using (SmtpClient Client = new SmtpClient(Config.SmtpServer, Config.SmtpPort))
            {
                Client.Credentials = new NetworkCredential(Config.SmtpUser, Config.SmtpPassword);
                Client.EnableSsl = true;
                using (MailMessage Message = new MailMessage())
                {
                    Message.From = new MailAddress(Config.From);
                    Message.To.Add(new MailAddress(Config.To));
                    Message.Subject = Subject;
                    Message.Body = Body;
                    Message.IsBodyHtml = true;

                    try
                    {
                        await Client.SendMailAsync(Message);
                    }
                    catch (SmtpException ex)
                    {
                        Console.WriteLine($"SMTP error: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        throw;
                    }
                }

            }
        }
        public async Task ConfirmEmailToSendOnSetup()
        {
            try
            {
                await SendEmail("Stock Quote Service Setup Confirmation", "The Stock Quote Service has been set up successfully.");
                Console.WriteLine("Confirmation email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
            }
        }
        public async Task PurchaseNotification(string TickerSymbol, decimal PurchasePoint, StockQuotation Quotation)
        {
            string Subject = $"Purchase Notification for {TickerSymbol}";
            string Body = $"The stock {TickerSymbol} has reached the purchase point of {PurchasePoint}.\n" +
                          $"Current price: {Quotation.Price} at {Quotation.Timestamp}";

            await SendEmail(Subject, Body);
        }
        public async Task SaleNotification(string TickerSymbol, decimal SalePoint, StockQuotation Quotation)
        {
            string Subject = $"Sale Notification for {TickerSymbol}";
            string Body = $"The stock {TickerSymbol} has reached the sale point of {SalePoint}.\n" +
                          $"Current price: {Quotation.Price} at {Quotation.Timestamp}";

            await SendEmail(Subject, Body);
        }
    }
}