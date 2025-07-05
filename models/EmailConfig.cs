namespace Quotation.Models
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}