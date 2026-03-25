using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        Task<Dictionary<int, Genre>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken);
        IQueryable<Genre> GetAllQueryable();
        Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken);
        Task<Genre?> Get(int id, CancellationToken cancellationToken);
        Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken);
        Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken);
        Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);
        Task<Dictionary<int, Genre>> GetGenresDictionary(CancellationToken cancellationToken);
    }
}
