using MediatR;

namespace WebApplication1.DTO.Notification
{
    public class ReviewChangedNotification:INotification
    {
        public int mediaId { get; set; }
    }
}
