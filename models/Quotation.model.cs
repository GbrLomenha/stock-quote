namespace Quotation.Models
{
    public class Quotation(string TickerSymbol, decimal Price, long Timestamp)
    {
        public string TickerSymbol { get; set; } = TickerSymbol;
        public decimal Price { get; set; } = Price;
        public long Timestamp { get; set; } = Timestamp;
    }
}