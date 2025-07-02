namespace Quotation.Models
{
    public class AppSettings(string ApiKey, int VerificationInterval, string EmailToSend)
    {
        public string ApiKey { get; set; } = ApiKey;
        public int VerificationInterval { get; set; } = VerificationInterval;
        public string EmailToSend { get; set; } = EmailToSend;
    }
}