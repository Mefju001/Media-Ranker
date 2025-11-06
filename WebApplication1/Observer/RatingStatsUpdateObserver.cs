using MediatR;
using WebApplication1.BackgroundService.Interfaces;
using WebApplication1.DTO.Notification;

namespace WebApplication1.Observer
{
    public class RatingStatsUpdateObserver : INotificationHandler<ReviewChangedNotification>
    {
        private readonly IBackgroundTaskQueue backgroundTaskQueue;
        public Task Handle(ReviewChangedNotification notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
