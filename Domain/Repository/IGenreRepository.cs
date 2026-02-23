using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        IQueryable<Genre> GetAllQueryable();
        Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken);
        Task<Genre?> Get(int id);
        Task<Genre?> FirstOrDefaultForNameAsync(string name);
        Task<Genre> AddAsync(Genre genre);
        Task<List<Genre>> GetByNamesAsync(List<string> names);
        Task<Dictionary<int, Genre>> GetGenresDictionary();
    }
}
