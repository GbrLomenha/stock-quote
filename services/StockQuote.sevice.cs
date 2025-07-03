using Quotation.Models;
namespace Quotation.Services
{
    public class StockQuoteService
    {
        public StockQuotation GetLatestStockQuotation(HttpClient Client, string TickerSymbol)
        {
            string Url = $"https://brapi.dev/api/quote/{TickerSymbol}?token={ApiSettings.ApiKey}";
            string Json = Client.GetStringAsync(Url).Result;

            //Make response tratable...

            ApiResponse Response = JsonService.ReadApiResponseJson(Json);

            return new StockQuotation(
                Response.Symbol,
                Response.RegularMarketPrice,
                DateTimeOffset.FromUnixTimeSeconds(Response.RegularMarketTime).UtcDateTime
            );
        }
    }
}