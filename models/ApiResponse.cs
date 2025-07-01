namespace Quotation.Models
{
    public class BrapiQuote
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("regularMarketPrice")]
        public decimal RegularMarketPrice { get; set; }

        [JsonProperty("regularMarketTime")]
        public long RegularMarketTime { get; set; }
    }
}