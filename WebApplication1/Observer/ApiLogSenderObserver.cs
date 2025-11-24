using MediatR;
using System.Text.Json;
using WebApplication1.DTO.Notification;

namespace WebApplication1.Observer
{
    public class ApiLogSenderObserver : INotificationHandler<LogNotification>
    {
        private readonly HttpClient _httpClient;
        private readonly string _logApiUrl;
        public ApiLogSenderObserver(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logApiUrl = configuration["LoggingApi:Url"];
        }
        public async Task Handle(LogNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            try
            {
                var jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                System.Text.Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_logApiUrl}/api/logs", jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Awaryjny log]: " + ex.ToString());
            }
        }
    }
}
