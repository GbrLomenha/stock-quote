namespace Quotation.Models
{
    public class BrapiQuote(string Symbol, decimal RegularMarketPrice, long RegularMarketTime)
    {
        public string Symbol { get; set; } = Symbol;
        public decimal RegularMarketPrice { get; set; } = RegularMarketPrice;
        public long RegularMarketTime { get; set; } = RegularMarketTime;
    }
}