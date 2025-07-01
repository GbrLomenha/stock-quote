namespace Quotation.Models
{
    public class ApiConfiguration
    {
        public string ApiKey { get; set; }
    }

    public class MonitorConfiguration
    {
        public int VerificationInterval { get; set; }
    }
    public class AppConfiguration
    {
        public ApiConfiguration Api { get; set; }
        public MonitorConfiguration Monitor { get; set; }
        
    }
}