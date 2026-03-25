namespace Domain.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(string message) : base(message) { }
    }
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class InvalidRefreshTokenException : BaseException
    {
        public InvalidRefreshTokenException(string message) : base(message) { }
    }
    public class UserClaimNotFoundException : BaseException
    {
        public UserClaimNotFoundException(string message) : base(message) { }
    }
    public class InvalidCredentialsException : BaseException
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
    public class UserNotFoundException : BaseException
    {
        public UserNotFoundException(string message) : base(message) { }
    }
    public class PasswordMismatchException : BaseException
    {
        public PasswordMismatchException(string message) : base(message) { }
    }
    public class NewPasswordIsSameAsOldException : BaseException
    {
        public NewPasswordIsSameAsOldException(string message) : base(message) { }
    }
    public class EmailAlreadyExistsException : BaseException
    {
        public EmailAlreadyExistsException(string message) : base(message) { }
    }
    public class DomainException : BaseException
    {
        public DomainException(string message) : base(message) { }
    }
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message) { }
    }
}
