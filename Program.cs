using Quotation.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

        // Validate input arguments
        var (Symbol, PurchasePoint, SalePoint) = InputTreatment.SanitizeInput(args);

        // Create Host
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddScoped<StockQuoteService>();
                services.AddScoped<EmailService>();
            })
            .Build();

        //Send confirmation email
        EmailService emailService = host.Services.GetRequiredService<EmailService>();
        await emailService.ConfirmEmailToSendOnSetup();

        // Start Stock Quote Service
        var StockQuoteService = host.Services.GetRequiredService<StockQuoteService>();
        await StockQuoteService.MonitorStockQuotation(Symbol, PurchasePoint, SalePoint);
    }
}