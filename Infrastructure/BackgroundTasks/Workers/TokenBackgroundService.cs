using Application.Common.Interfaces;
using Infrastructure.BackgroundTasks.CleanService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundTasks.Workers
{
    public class TokenBackgroundService : IHostedService, IDisposable
    {
        public readonly ILogger<TokenBackgroundService> logger;
        public readonly IServiceScopeFactory ServiceProvider;
        private Timer timer;

        private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30);
        public TokenBackgroundService(ILogger<TokenBackgroundService> logger, IServiceScopeFactory serviceProvider)
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
            timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), _cleanupInterval);
            return Task.CompletedTask;
        }
        private void DoWork(object? state)
        {
            logger.LogInformation("Zaczynamy czyszczenie bazy danych z tokenów");
            using (var scope = ServiceProvider.CreateScope())
            {
                var cleanupService = scope.ServiceProvider.GetRequiredService<ITokenCleanService>();
                cleanupService.CleanTokens().Wait();
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
