namespace Application.Features.AuthServices.CleanTokens
{
    public interface ITokenCleanService
    {
        Task CleanTokens(CancellationToken cancellationToken);
    }
}
