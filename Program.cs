using Quotation.Models;
using Quotation.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

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

        decimal PurshacePoint = decimal.Parse(args[1]);
        decimal SalePoint = decimal.Parse(args[2]);

        Console.WriteLine("Starting Stock Quote Service...");
        var stockQuoteService = host.Services.GetRequiredService<StockQuoteService>();
        await stockQuoteService.MonitorStockQuotation(args[0], PurshacePoint, SalePoint);
    }
}