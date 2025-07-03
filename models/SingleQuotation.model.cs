namespace Quotation.Models
{
    public class SingleQuotation(string TickerSymbol, decimal Price, DateTime Timestamp)
    {
        public string TickerSymbol { get; set; } = TickerSymbol;
        public decimal Price { get; set; } = Price;
        public DateTime Timestamp { get; set; } = Timestamp;
    }
}