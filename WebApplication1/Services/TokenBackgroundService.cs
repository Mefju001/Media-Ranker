using WebApplication1.Interfaces;

namespace WebApplication1.Services
{
    public class TokenBackgroundService : IHostedService, IDisposable
    {
        public readonly ILogger<TokenBackgroundService> logger;
        public readonly IServiceProvider ServiceProvider;
        private Timer timer;

        private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30);
        public TokenBackgroundService(ILogger<TokenBackgroundService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            ServiceProvider = serviceProvider;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Zaczynam czyszczenie bazy danych z tokenów nieaktywnych");
            timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), _cleanupInterval);
            return Task.CompletedTask;
        }
        private void DoWork(object? state)
        {
            logger.LogInformation("Zaczynamy czyszczenie bazy danych z tokenów");
            using (var scope = ServiceProvider.CreateScope())
            {
                var cleanupService = scope.ServiceProvider.GetRequiredService<ITokenCleanupService>();
                cleanupService.Cleanup().Wait();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("zatrzymanie czyszczenia bazy danych z tokenów nieaktywnych");
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
