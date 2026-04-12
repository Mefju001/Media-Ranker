using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository:IRepository<Genre,int>
    {
        Task<Dictionary<int, Genre>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken);
        Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken);
        Task<Dictionary<int, Genre>> GetGenresDictionary(CancellationToken cancellationToken);
    }
}
