using MediatR;

namespace WebApplication1.DTO.Request
{
    public class LogNotification:INotification
    {
        public string Level { get; }
        public string Message { get; }
        public string Source { get; }
        public LogNotification(string level, string message, string source)
        {
            Level = level;
            Message = message;
            Source = source;
        }
    }
}
