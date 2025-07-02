using Quotation.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

        //1- read appsettings.json file
        AppSettings AppSettings = ConfigService.Config() ?? throw new FileNotFoundException("AppConfig.json file not found. Please ensure it exists in the application directory.");

        Console.WriteLine(AppSettings.EmailToSend);

        // //2- deserialize json file to a new AppConfig object

        // //3- print if wrong values are found

        // Console.WriteLine("Configurations loaded successfully!");

        // Console.WriteLine("verifying initial parameters...");

        // //4- verify initial parameters

        // //5- print if wrong values are found

        // Console.WriteLine("Initial parameters verified successfully!");

        // //6- convert types of parameters

        // //7- print initial configurations (email, stock, values and interval)

        // //8- start the Stock Quote Service
    }
}