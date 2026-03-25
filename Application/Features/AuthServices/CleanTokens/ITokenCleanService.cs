namespace Infrastructure.BackgroundTasks.CleanService
{
    public interface ITokenCleanService
    {
        Task CleanTokens(CancellationToken cancellationToken);
    }
}
