using Quotation.Models;
using System.Text.Json;

public static class ConfigService
{
    public static AppSettings? Config()
    {
        string json;
        try
        {
            json = File.ReadAllText("AppSettings.json");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("AppConfig.json file not found. Please ensure it exists in the application directory.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the AppConfig.json file: {ex.Message}");
            return null;
        }
        AppSettings? AppSettings = JsonSerializer.Deserialize<AppSettings>(json);
        return AppSettings;
    }
}