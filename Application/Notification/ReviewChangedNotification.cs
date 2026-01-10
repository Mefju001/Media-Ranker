using MediatR;

namespace WebApplication1.Application.Notification
{
    public class ReviewChangedNotification : INotification
    {
        public int mediaId { get; set; }
    }
}
