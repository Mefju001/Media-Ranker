
namespace WebApplication1.Services
{
    public class TokenCleanupService : IHostedService, IDisposable
    {
        public readonly ILogger<TokenCleanupService> logger;
        public readonly IServiceProvider ServiceProvider;
        private Timer timer;

        private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30);
        private readonly TimeSpan _tokenRetentionTime = TimeSpan.FromDays(30);
        public TokenCleanupService(ILogger<TokenCleanupService> logger, IServiceProvider serviceProvider, Timer timer)
        {
            this.logger = logger;
            ServiceProvider = serviceProvider;
            this.timer = timer;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Zaczynam czyszczenie bazy danych z tokenów nieaktywnych");
            timer = new Timer(doWork,null,TimeSpan.FromMinutes(1),_cleanupInterval);
            return Task.CompletedTask;
        }

        private void doWork(object? state)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("zatrzymanie czyszczenia bazy danych z tokenów nieaktywnych");
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
