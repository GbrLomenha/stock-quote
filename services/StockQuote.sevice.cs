using System.Net;
using System.Net.Http.Json;
using Quotation.Models;
namespace Quotation.Services
{
    public class StockQuoteService
    {
        private readonly IHttpClientFactory ClientFactory;

        public StockQuoteService(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<StockQuotation> GetLatestStockQuotation(string TickerSymbol)
        {
            HttpClient Client = ClientFactory.CreateClient();
            Uri Url = new($"https://brapi.dev/api/quote/{TickerSymbol}?token={ApiSettings.ApiKey}");
            HttpResponseMessage Response = await Client.GetAsync(Url);
            if (!Response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch stock quotation. Status code: {Response.StatusCode}");
                Environment.Exit(1);
            }

            ApiResponse? ApiResponse = await Response.Content.ReadFromJsonAsync<ApiResponse>();
            if (ApiResponse == null)
                throw new Exception("Failed to deserialize API response.");

            Console.WriteLine($"API Response: {ApiResponse.Symbol} - {ApiResponse.RegularMarketPrice} at {ApiResponse.RegularMarketTime}");

            return new StockQuotation(
                ApiResponse.Symbol,
                ApiResponse.RegularMarketPrice,
                DateTimeOffset.FromUnixTimeSeconds(ApiResponse.RegularMarketTime).UtcDateTime
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

                await Task.Delay(60000); // Wait for 60 seconds before the next request
            }
        }
    }
}