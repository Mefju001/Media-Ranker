namespace Application.Common.Interfaces
{
    public interface IStatsUpdateService
    {
        Task update(int mediaId, CancellationToken cancellationToken);
    }
}
