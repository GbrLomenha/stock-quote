namespace Quotation.Models
{
    public class ApiResponse
    {
        public required string Symbol { get; set; }
        public required decimal RegularMarketPrice { get; set; }
        public required DateTime RegularMarketTime { get; set; }
        public required string LogoUrl { get; set; }

        public string LongName { get; set; } = string.Empty;
    }
    public class ApiRootResponse
    {
        public List<ApiResponse> Results { get; set; }

    }
}