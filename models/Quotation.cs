namespace Quotation.Models
{
    public class Quotation
    {
        public string TickerSymbol { get; set; }
        public decimal Price { get; set; }
        public long Timestamp { get; set; }
    }
}