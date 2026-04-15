using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository : IRepository<Genre, Guid>
    {
        Task<Dictionary<Guid, Genre>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
        Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);
        Task<Dictionary<Guid, Genre>> GetGenresDictionary(CancellationToken cancellationToken);
    }
}
