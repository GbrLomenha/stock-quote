class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Inicializing Stock Quote Service... to {args[0]}");

        Console.WriteLine("Reading configurations from appsettings.json...");

        //1- load json file

        //2- deserialize json file to a new AppConfig object

        //3- print if wrong values are found

        Console.WriteLine("Configurations loaded successfully!");

        Console.WriteLine("verifying initial parameters...");

        //4- verify initial parameters

        //5- print if wrong values are found

        Console.WriteLine("Initial parameters verified successfully!");

        //6- convert types of parameters

        //7- print initial configurations (email, stock, values and interval)

        //8- start the Stock Quote Service
    }
}