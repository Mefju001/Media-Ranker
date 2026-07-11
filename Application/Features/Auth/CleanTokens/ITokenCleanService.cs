namespace Application.Features.Auth.CleanTokens
{
    public interface ITokenCleanService
    {
        Task CleanTokens(CancellationToken cancellationToken);
    }
}
