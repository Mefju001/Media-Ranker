using MediatR;
using WebApplication1.BackgroundService.Interfaces;
using WebApplication1.DTO.Notification;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Observer
{
    public class RatingStatsUpdateObserver : INotificationHandler<ReviewChangedNotification>
    {
        private readonly IBackgroundTaskQueue backgroundTaskQueue;
        public readonly IServiceProvider ServiceProvider;


        public RatingStatsUpdateObserver(IBackgroundTaskQueue backgroundTaskQueue, IServiceProvider serviceProvider)
        {
            this.backgroundTaskQueue = backgroundTaskQueue;
            this.ServiceProvider = serviceProvider;
        }

        public Task Handle(ReviewChangedNotification notification, CancellationToken cancellationToken)
        {
            ValueTask value = backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (token) =>
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var updateService = scope.ServiceProvider.GetRequiredService<IStatsUpdateService>();
                    await updateService.update(notification.mediaId, token);
                }
            });
            return Task.CompletedTask;
        }
    }
}
