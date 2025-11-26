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
    public class InvalidCredentialsException:Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
        public int ErrorCode { get; } = 4003;
    }
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
        public int ErrorCode { get; } = 4004;
    }
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException(string message) : base(message) { }
        public int ErrorCode { get; } = 4005;
    }
    public class NewPasswordIsSameAsOldException : Exception
    {
        public NewPasswordIsSameAsOldException(string message) : base(message) { }
        public int ErrorCode { get; } = 4006;
    }
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string message) : base(message) { }
        public int ErrorCode { get; } = 4007;
    }
}
