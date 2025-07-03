using Quotation.Models;
namespace Quotation.Services
{
    public class StockQuoteService
    {
        public async Task<StockQuotation> GetLatestStockQuotation(HttpClient Client, string TickerSymbol)
        {
            string Url = $"https://brapi.dev/api/quote/{TickerSymbol}?token={ApiSettings.ApiKey}";
            string Json = await Client.GetStringAsync(Url);

            //Make response tratable...

            ApiResponse Response = JsonService.ReadApiResponseJson(Json);

            return new StockQuotation(
                Response.Symbol,
                Response.RegularMarketPrice,
                DateTimeOffset.FromUnixTimeSeconds(Response.RegularMarketTime).UtcDateTime
            );
        }

        async public void MonitorStockQuotation(string TickerSymbol, decimal PurshacePoint, decimal SalePoint)
        {
            HttpClient Client = new HttpClient();
            Client.Timeout = TimeSpan.FromSeconds(10);

            while (true)
            {
                StockQuotation ActualQuotation = await GetLatestStockQuotation(Client, TickerSymbol);
                
                if(ActualQuotation.Price<= PurshacePoint)
                {
                    Console.WriteLine($"Stock {TickerSymbol} has reached the purchase point of {PurshacePoint}. Consider buying.");
                }
                else if(ActualQuotation.Price>= SalePoint)
                {
                    Console.WriteLine($"Stock {TickerSymbol} has reached the sale point of {SalePoint}. Consider selling.");
                }
                else
                {
                    Console.WriteLine($"Stock {TickerSymbol} is at {ActualQuotation.Price}. No action needed.");
                }

                Thread.Sleep(60000); // Wait for 60 seconds before the next request
            }
        }
    }
}