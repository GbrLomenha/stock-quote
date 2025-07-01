namespace Quotation.Services
{
    public class ApiQuotation
    {
        public decimal GetLatestQuotation(string tickerSymbol)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(tickerSymbol);

            return 0;
        }
    }
}