using Quotation.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

        var settings = JsonService.ReadConfigurationJson();

        ApiSettings.ApiKey = settings.ApiKey;


        // //3- print if wrong values are found

        // Console.WriteLine("Configurations loaded successfully!");

        // Console.WriteLine("verifying initial parameters...");

        // //4- verify initial parameters

        // //5- print if wrong values are found

        // Console.WriteLine("Initial parameters verified successfully!");

        // //6- convert types of parameters
        decimal PurshacePoint = decimal.Parse(args[1]);
        decimal SalePoint = decimal.Parse(args[2]);

        // //7- print initial configurations (email, stock, values and interval)

        // //8- start the Stock Quote Service
        Console.WriteLine("Starting Stock Quote Service...");

    }
}