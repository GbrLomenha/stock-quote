namespace Quotation.Models
{
    public class Configuration
    {
        public string? ApiKey { get; set; }
        public string? EmailToSend { get; set; }
        public int? VerificationInterval { get; set; }
    }
}