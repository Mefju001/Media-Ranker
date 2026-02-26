namespace Application.Common.Interfaces
{
    public interface ICurrentUserContext
    {
        Guid? Jti {  get; }
        Guid? UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
}
