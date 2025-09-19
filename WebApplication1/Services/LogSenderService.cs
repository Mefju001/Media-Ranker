using System.Text.Json;
using WebApplication1.DTO.Request;

namespace WebApplication1.Services
{
    public class LogSenderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LogSenderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendLogAsync(string Level, string Message, string Source)
        {
            var logApiUrl = _configuration["LoggingApi:Url"];
            var logEntry = new LogSender(Level, Message, Source);
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(logEntry),
                System.Text.Encoding.UTF8, "application/json");
            try
            {
                await _httpClient.PostAsync($"{logApiUrl}/api/logs", jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nie udało się wysłać logu do Logging API: {ex.Message}");
            }
        }
    }
}
