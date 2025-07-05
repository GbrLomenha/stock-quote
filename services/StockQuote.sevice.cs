using System.Net.Http.Json;
using Quotation.Models;
using Microsoft.Extensions.Configuration;
namespace Quotation.Services
{
    public class StockQuoteService
    {
        private readonly IHttpClientFactory ClientFactory;
        private readonly string ApiKey;
        private readonly IConfiguration Configuration;
        private readonly EmailService EmailService;

        public StockQuoteService(IHttpClientFactory ClientFactory, IConfiguration Configuration, EmailService EmailService)
        {
            this.ClientFactory = ClientFactory ?? throw new ArgumentNullException(nameof(ClientFactory));
            ApiKey = Configuration["ApiKey"] ?? throw new ArgumentNullException("ApiKey", "API key is missing in configuration.");
            this.Configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
            this.EmailService = EmailService;
        }

        public async Task<StockQuotation> GetLatestStockQuotation(string TickerSymbol)
        {
            HttpClient Client = ClientFactory.CreateClient();
            Uri Url = new($"https://brapi.dev/api/quote/{TickerSymbol}?token={ApiKey}");
            HttpResponseMessage Response = await Client.GetAsync(Url);
            if (!Response.IsSuccessStatusCode)
                Console.WriteLine($"Failed to fetch stock quotation. Status code: {Response.StatusCode}");

            ApiRootResponse? ApiRootResponse = await Response.Content.ReadFromJsonAsync<ApiRootResponse>() ?? throw new Exception("Failed to deserialize API response.");

            ApiResponse ApiResponse = ApiRootResponse.Results[0] ?? throw new Exception("No valid stock data found in API response.");

            return new StockQuotation(
                ApiResponse.Symbol,
                ApiResponse.RegularMarketPrice,
                ApiResponse.RegularMarketTime
            );
        }

        public async Task MonitorStockQuotation(string TickerSymbol, decimal PurshacePoint, decimal SalePoint)
        {
            HttpClient Client = ClientFactory.CreateClient();
            Client.Timeout = TimeSpan.FromSeconds(10);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            while (true)
            {
                StockQuotation ActualQuotation = await GetLatestStockQuotation(TickerSymbol);
                if (ActualQuotation.Price <= PurshacePoint) {
                    Console.WriteLine($"Stock {TickerSymbol} has reached the purchase point of {PurshacePoint}. Consider buying.");
                    EmailService.PurchaseNotification(TickerSymbol, PurshacePoint, ActualQuotation);
                }
                else if (ActualQuotation.Price >= SalePoint) {
                    Console.WriteLine($"Stock {TickerSymbol} has reached the sale point of {SalePoint}. Consider selling.");
                    EmailService.SaleNotification(TickerSymbol, SalePoint, ActualQuotation);
                }
                else
                    Console.WriteLine($"Stock {TickerSymbol} is at {ActualQuotation.Price}. No action needed.");

                await Task.Delay(TimeSpan.FromMinutes(int.Parse(Configuration["VerificationInterval"] ?? "30")));
            }
        }
    }
}