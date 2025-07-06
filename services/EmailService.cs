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
			Console.WriteLine("Sending confirmation email...");
			try
			{
				string subject = "Stock Quote Service Setup Confirmation";
				string body = $@"
					<html>
					<head>
						<style>
						body {{
							font-family: Arial, Helvetica, sans-serif;
							background-color: #f5f6fa;
							color: #222;
							margin: 0;
							padding: 0;
						}}
						.container {{
							background: #fff;
							max-width: 480px;
							margin: 40px auto;
							border-radius: 8px;
							box-shadow: 0 2px 8px rgba(0,0,0,0.07);
							padding: 32px 24px;
						}}
						.title {{
							color: #444;
							font-size: 1.3em;
							margin-bottom: 16px;
						}}
						.info {{
							background: #f0f1f3;
							border-radius: 6px;
							padding: 16px;
							margin-bottom: 24px;
							text-align: center;
						}}
						.footer {{
							color: #888;
							font-size: 0.95em;
							margin-top: 32px;
							text-align: center;
						}}
						.logo {{
							display: block;
							margin: 0 auto 16px auto;
							max-width: 80px;
							max-height: 80px;
							border-radius: 8px;
							background: #fff;
							box-shadow: 0 1px 4px rgba(0,0,0,0.08);
						}}
						</style>
					</head>
					<body>
						<div class='container'>
						<div class='title'>Stock Quote Service</div>
						<div class='info'>
							The service has been configured successfully!<br/>
							You will now receive automatic stock quote notifications via email.
						</div>
						<div>
							If you do not recognize this setting, please review your credentials and security settings.
						</div>
						<div class='footer'>
							Stock Quote Service &copy; {DateTime.Now.Year}
						</div>
						</div>
					</body>
					</html>";

				await SendEmail(subject, body);
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
			string Body = $@"
				<html>
				<head>
					<style>
					body {{
						font-family: Arial, Helvetica, sans-serif;
						background-color: #f5f6fa;
						color: #222;
						margin: 0;
						padding: 0;
					}}
					.container {{
						background: #fff;
						max-width: 480px;
						margin: 40px auto;
						border-radius: 8px;
						box-shadow: 0 2px 8px rgba(0,0,0,0.07);
						padding: 32px 24px;
					}}
					.title {{
						color: #444;
						font-size: 1.3em;
						margin-bottom: 16px;
					}}
					.info {{
						background: #f0f1f3;
						border-radius: 6px;
						padding: 16px;
						margin-bottom: 24px;
					}}
					.footer {{
						color: #888;
						font-size: 0.95em;
						margin-top: 32px;
						text-align: center;
					}}
					.logo {{
						display: block;
						margin: 0 auto 8px auto;
						max-width: 64px;
						max-height: 64px;
						border-radius: 8px;
						background: #fff;
						box-shadow: 0 1px 4px rgba(0,0,0,0.08);
					}}
					.longname {{
						text-align: center;
						color: #555;
						font-size: 1.05em;
						margin-bottom: 18px;
					}}
					</style>
				</head>
				<body>
					<div class='container'>
					<img class='logo' src='https://icons.brapi.dev/icons/{TickerSymbol}.svg' alt='Logo {Quotation.LongName}' />
					<div class='longname'>{Quotation.LongName}</div>
					<div class='title'>Stock Purchase Alert</div>
					<div class='info'>
						<strong>{Quotation.LongName} ({TickerSymbol})</strong> has reached the purchase point of <strong>{PurchasePoint}</strong>.<br/>
						<br/>
						<b>Current price:</b> {Quotation.Price}<br/>
						<b>Date/Time:</b> {Quotation.Timestamp:dd/MM/yyyy HH:mm:ss}
					</div>
					<div>
						Consider buying according to your investment strategy.
					</div>
					<div class='footer'>
						Stock Quote Service &copy; {DateTime.Now.Year}
					</div>
					</div>
				</body>
				</html>";

			await SendEmail(Subject, Body);
		}

		public async Task SaleNotification(string TickerSymbol, decimal SalePoint, StockQuotation Quotation)
		{
			string Subject = $"Sale Notification for {TickerSymbol}";
			string Body = $@"
				<html>
				<head>
					<style>
					body {{
						font-family: Arial, Helvetica, sans-serif;
						background-color: #f5f6fa;
						color: #222;
						margin: 0;
						padding: 0;
					}}
					.container {{
						background: #fff;
						max-width: 480px;
						margin: 40px auto;
						border-radius: 8px;
						box-shadow: 0 2px 8px rgba(0,0,0,0.07);
						padding: 32px 24px;
					}}
					.title {{
						color: #444;
						font-size: 1.3em;
						margin-bottom: 16px;
					}}
					.info {{
						background: #f0f1f3;
						border-radius: 6px;
						padding: 16px;
						margin-bottom: 24px;
					}}
					.footer {{
						color: #888;
						font-size: 0.95em;
						margin-top: 32px;
						text-align: center;
					}}
					.logo {{
						display: block;
						margin: 0 auto 8px auto;
						max-width: 64px;
						max-height: 64px;
						border-radius: 8px;
						background: #fff;
						box-shadow: 0 1px 4px rgba(0,0,0,0.08);
					}}
					.longname {{
						text-align: center;
						color: #555;
						font-size: 1.05em;
						margin-bottom: 18px;
					}}
					</style>
				</head>
				<body>
					<div class='container'>
					<img class='logo' src='https://icons.brapi.dev/icons/{TickerSymbol}.svg' alt='Logo {Quotation.LongName}' />
					<div class='longname'>{Quotation.LongName}</div>
					<div class='title'>Stock Sale Alert</div>
					<div class='info'>
						<strong>{Quotation.LongName} ({TickerSymbol})</strong> has reached the sale point of <strong>{SalePoint}</strong>.<br/>
						<br/>
						<b>Current price:</b> {Quotation.Price}<br/>
						<b>Date/Time:</b> {Quotation.Timestamp:dd/MM/yyyy HH:mm:ss}
					</div>
					<div>
						Consider selling according to your investment strategy.
					</div>
					<div class='footer'>
						Stock Quote Service &copy; {DateTime.Now.Year}
					</div>
					</div>
				</body>
				</html>";

			await SendEmail(Subject, Body);
		}
	}
}