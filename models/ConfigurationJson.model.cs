namespace Quotation.Models
{
    public interface ConfigurationJson
    {
        public string ApiKey { get; set; }
        public string EmailToSend { get; set; }
        public int VerificationInterval { get; set; }
    }
}