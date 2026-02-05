using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        IQueryable<GenreDomain> GetAllQueryable();
        Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<GenreDomain>> GetAllAsync(CancellationToken cancellationToken);
        Task<GenreDomain?> Get(int id);
        Task<GenreDomain?> FirstOrDefaultForNameAsync(string name);
        Task<GenreDomain> AddAsync(GenreDomain genre);
        Task<List<GenreDomain>> GetByNamesAsync(List<string> names);
        Task<Dictionary<int, GenreDomain>> GetGenresDictionary();
    }
}
