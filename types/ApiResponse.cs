namespace Quotation.Models
{
    public class ApiResponse
    {
        public required string Symbol { get; set; }
        public required decimal RegularMarketPrice { get; set; }
        public required long RegularMarketTime { get; set; }
    }
}