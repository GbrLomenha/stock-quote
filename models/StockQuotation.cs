namespace Quotation.Models
{
    public class StockQuotation(string TickerSymbol, decimal Price, DateTime Timestamp, string LogoUrl, string LongName)
    {
        public string TickerSymbol { get; set; } = TickerSymbol;
        public decimal Price { get; set; } = Price;
        public DateTime Timestamp { get; set; } = Timestamp;
        public string LogoUrl { get; set; } = LogoUrl;
        public string LongName { get; set; } = LongName;
    }
}