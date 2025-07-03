namespace Quotation.Models
{
    public class Configuration
    {
        public required string ApiKey { get; set; }
        public required string EmailToSend { get; set; }
        public required int VerificationInterval { get; set; }
    }
}