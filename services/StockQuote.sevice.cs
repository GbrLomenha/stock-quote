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

        public StockQuoteService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            ClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            ApiKey = configuration["ApiKey"] ?? throw new ArgumentNullException("ApiKey", "API key is missing in configuration.");
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                if (ActualQuotation.Price <= PurshacePoint)
                    Console.WriteLine($"Stock {TickerSymbol} has reached the purchase point of {PurshacePoint}. Consider buying.");
                else if (ActualQuotation.Price >= SalePoint)
                    Console.WriteLine($"Stock {TickerSymbol} has reached the sale point of {SalePoint}. Consider selling.");
                else
                    Console.WriteLine($"Stock {TickerSymbol} is at {ActualQuotation.Price}. No action needed.");

                await Task.Delay(TimeSpan.FromMinutes(int.Parse(Configuration["VerificationInterval"] ?? "30")));
            }
        }
    }
}