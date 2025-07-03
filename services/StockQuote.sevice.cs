using Quotation.Models;
namespace Quotation.Services
{
    public class StockQuoteService
    {
        public SingleQuotation GetLatestStockQuotation(HttpClient Client, string TickerSymbol)
        {
            string url = $"https://brapi.dev/api/quote/{TickerSymbol}?token={ApiSettings.ApiKey}";
            string response = Client.GetStringAsync(url).Result;

            //Make response tratable...

            ApiResponse apiResponse = JsonService.ReadApiResponseJson(response);

            return new SingleQuotation(
                apiResponse.Symbol,
                apiResponse.RegularMarketPrice,
                DateTimeOffset.FromUnixTimeSeconds(apiResponse.RegularMarketTime).UtcDateTime
            );
        }
    }
}