using System.Text.Json;

namespace AstronomyPictureOfTheDayNotifier
{
    public class ConfigurationService
    {
        public static EmailSettings? GetEmailSettings()
        {
            try
            {
                var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "appconfig.json");
                var config = JsonSerializer.Deserialize<Dictionary<string, EmailSettings>>(File.ReadAllText(configPath));
                if (config != null && config.TryGetValue("SmtpClient", out var emailSettings))
                {
                    return emailSettings;
                }

                Console.WriteLine("SmtpClient configuration not found or invalid");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration: {ex.Message}");
                return null;
            }
        }
    }
}
