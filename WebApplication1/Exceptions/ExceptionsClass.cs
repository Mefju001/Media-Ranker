namespace WebApplication1.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException(string message) : base(message) { }
        public int ErrorCode { get; } = 4001;
    }
    public class UserClaimNotFoundException : Exception
    {
        public UserClaimNotFoundException(string message) : base(message) { }
        public int ErrorCode { get; } = 4002;
    }
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
        public int ErrorCode { get; } = 4004;
    }
}
