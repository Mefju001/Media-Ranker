namespace WebApplication1.Services.Interfaces
{
    public interface IStatsUpdateService
    {
        Task update(int mediaId, CancellationToken cancellationToken);
    }
}
