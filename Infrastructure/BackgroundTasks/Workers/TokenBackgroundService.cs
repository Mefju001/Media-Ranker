using Application.Features.AuthServices.CleanTokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundTasks.Workers
{
    public class TokenBackgroundService : BackgroundService
    {
        private readonly ILogger<TokenBackgroundService> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly TimeSpan cleanupInterval = TimeSpan.FromDays(30);

        public TokenBackgroundService(ILogger<TokenBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("TokenBackgroundService is starting.");
            await DoCleanupWork(stoppingToken);
            using PeriodicTimer timer = new(cleanupInterval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoCleanupWork(stoppingToken);
            }
        }
        private async Task DoCleanupWork(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("TokenBackgroundService is running cleanup task.");
                using var scope = scopeFactory.CreateScope();
                var tokenCleanService = scope.ServiceProvider.GetRequiredService<ITokenCleanService>();
                await tokenCleanService.CleanTokens(stoppingToken);
                logger.LogInformation("TokenBackgroundService completed cleanup task.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while cleaning tokens.");
            }
        }
    }
}
