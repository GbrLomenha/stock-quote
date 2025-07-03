using Quotation.Models;
using System.Text.Json;

public static class JsonService
{
    public static Configuration ReadConfigurationJson()
    {
        string Json;
        try
        {
            Json = File.ReadAllText("AppSettings.Json");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("AppConfig.Json file not found. Please ensure it exists in the application directory.");
            Environment.Exit(1);
            return null; // Esta linha é necessária para satisfazer o compilador.
        }
        Configuration? ObjectJson = JsonSerializer.Deserialize<Configuration>(Json)
            ?? throw new Exception("Failed to deserialize the AppConfig.Json file. Please ensure it is in the correct format.");

        return ObjectJson;
    }

    public static ApiResponse ReadApiResponseJson(string json)
    {
        try
        {
            ApiResponse Response = JsonSerializer.Deserialize<ApiResponse>(json)
                ?? throw new Exception("Failed to deserialize the API response JSON. Please ensure it is in the correct format.");
            return Response;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"An error occurred while deserializing the API response JSON: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }
}