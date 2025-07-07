namespace Quotation.Services
{
    public static class InputTreatment
    {
        public static (string Symbol, decimal PurchasePoint, decimal SalePoint) SanitizeInput(string[] args)
        {
            Console.WriteLine("Sanitizing input...");
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No input provided. The run command needs: <symbol> <purchasePoint> <salePoint>");
                Environment.Exit(1);
            }

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid number of arguments. The run command needs: <symbol> <purchasePoint> <salePoint>");
                Environment.Exit(1);
            }
            if (string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("The stock symbol cannot be empty.");
                Environment.Exit(1);
            }
            if (!decimal.TryParse(args[1], out decimal PurchasePoint) || PurchasePoint <= 0)
            {
                Console.WriteLine("Invalid purchase point. It must be a positive decimal number.");
                Environment.Exit(1);
            }
            if (!decimal.TryParse(args[2], out decimal SalePoint) || SalePoint <= 0)
            {
                Console.WriteLine("Invalid sale point. It must be a positive decimal number.");
                Environment.Exit(1);
            }
            if (PurchasePoint >= SalePoint)
            {
                Console.WriteLine("Purchase point must be less than sale point.");
                Environment.Exit(1);
            }
            Console.WriteLine($"Input sanitized: Symbol={args[0]}, PurchasePoint={PurchasePoint}, SalePoint={SalePoint}");
            return (args[0], PurchasePoint, SalePoint);
        }
    }
}   