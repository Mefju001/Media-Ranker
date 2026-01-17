using MediatR;

namespace Application.Notification
{
    public class ReviewChangedNotification : INotification
    {
        public int mediaId { get; set; }
    }
}
