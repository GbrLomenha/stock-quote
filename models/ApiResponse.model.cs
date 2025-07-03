namespace Quotation.Models
{
    public interface ApiResponse
    {
        public string Symbol { get; set; }
        public decimal RegularMarketPrice { get; set; }
        public long RegularMarketTime { get; set; }
    }
}