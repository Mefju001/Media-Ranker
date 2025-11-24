using WebApplication1.BackgroundTasks.Interfaces;
using Host = Microsoft.Extensions.Hosting;

namespace WebApplication1.BackgroundTasks.Workers
{
    public class QueueProcessorService : Host.BackgroundService
    {
        private readonly ILogger<QueueProcessorService> logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly IBackgroundTaskQueue backgroundTaskQueue;

        public QueueProcessorService(ILogger<QueueProcessorService> logger, IServiceProvider serviceProvider, IBackgroundTaskQueue backgroundTaskQueue)
        {
            this.logger = logger;
            ServiceProvider = serviceProvider;
            this.backgroundTaskQueue = backgroundTaskQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workItem = await backgroundTaskQueue.DequeueAsync(stoppingToken);
                    using var scope = ServiceProvider.CreateScope();
                    await workItem(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("End");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError("Fail " + ex.ToString());
                }
            }
        }
    }
}
