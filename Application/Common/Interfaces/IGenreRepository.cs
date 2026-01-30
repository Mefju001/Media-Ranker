using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        GenreDomain? Get(int id);
        Task<GenreDomain?> FirstOrDefaultForNameAsync(string name);
        Task<GenreDomain> AddAsync(GenreDomain genre);
        Task<List<GenreDomain>> GetByNamesAsync(List<string> names);
    }
}
