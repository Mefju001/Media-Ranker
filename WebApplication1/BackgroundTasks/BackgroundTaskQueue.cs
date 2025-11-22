
using System.Threading.Channels;
using WebApplication1.BackgroundService.Interfaces;

namespace WebApplication1.BackgroundService
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly int capacity = 100;
        public readonly ILogger<BackgroundTaskQueue> logger;
        private readonly Channel<Func<CancellationToken, ValueTask>> queue;
        public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
        {
            this.logger = logger;
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Dequeue");
            Func<CancellationToken, ValueTask>? workItem = await queue.Reader.ReadAsync(cancellationToken);
            return workItem;
        }

        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem, nameof(workItem));
            await queue.Writer.WriteAsync(workItem);
        }
    }
}
