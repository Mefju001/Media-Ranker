using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository:IRepository<Media>
    {
        Task<List<T>> GetByTypeAsync<T>(CancellationToken ct) where T : Media;
        Task<T?> GetByIdAsync<T>(int id, CancellationToken ct) where T : Media;
        Task<T> AddAsync<T>(T entity, CancellationToken ct) where T: Media;
        Task<T?> GetByIdWithDetailsAsync<T>(int id, CancellationToken ct) where T : Media;
        Task<List<T>> GetAll<T>(CancellationToken cancellationToken) where T : Media;
        IQueryable<T>GetAsQueryable<T>() where T : Media;
        Task<List<T>> FromAsQueryableToList<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : Media;
        Task<Dictionary<int, Media>> GetByIds(List<int> mediaIds, CancellationToken cancellationToken);
    }
}
