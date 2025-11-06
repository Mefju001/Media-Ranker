
using WebApplication1.Services.Interfaces;
using Host = Microsoft.Extensions.Hosting;

namespace WebApplication1.BackgroundTasks.Service
{
    public class QueueProcessorService: Host.BackgroundService
    {
        public readonly ILogger<QueueProcessorService> logger;
        public readonly IServiceProvider ServiceProvider;

        public QueueProcessorService(ILogger<QueueProcessorService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            ServiceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private void DoWork(object? state)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var statsUpdateSerivce = scope.ServiceProvider.GetRequiredService<IStatsUpdateService>();
                //statsUpdateSerivce.update.wait();
            }
            throw new NotImplementedException();
        }
    }
}
