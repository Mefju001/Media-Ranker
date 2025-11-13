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
        private readonly IServiceScopeFactory serviceScopeFactory;

        public RatingStatsUpdateObserver(IServiceScopeFactory factory, IBackgroundTaskQueue backgroundTaskQueue, IServiceProvider serviceProvider)
        {
            serviceScopeFactory = factory;
            this.backgroundTaskQueue = backgroundTaskQueue;
            this.ServiceProvider = serviceProvider;
        }

        public Task Handle(ReviewChangedNotification notification, CancellationToken cancellationToken)
        {
            ValueTask value = backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (token) =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var updateService = scope.ServiceProvider.GetRequiredService<IStatsUpdateService>();
                    await updateService.update(notification.mediaId, token);
                }
            });
            return Task.CompletedTask;
        }
    }
}
