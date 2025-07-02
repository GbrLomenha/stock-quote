using Quotation.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

        //1- read appsettings.json file
        AppSettings AppSettings = ConfigService.ReadJson() ?? throw new FileNotFoundException("AppConfig.json file not found. Please ensure it exists in the application directory.");

        Console.WriteLine(AppSettings.EmailToSend);

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
        Console.WriteLine($"Email: {AppSettings.EmailToSend}");
        Console.WriteLine($"Ticker Symbol: {args[0]}");
        Console.WriteLine($"Purchase Point: {PurshacePoint}");
        Console.WriteLine($"Sale Point: {SalePoint}");
        Console.WriteLine($"Interval: {AppSettings.VerificationInterval} minute");

        // //8- start the Stock Quote Service
    }
}