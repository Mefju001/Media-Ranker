using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieDomain>> GetAllAsync();
        Task<MovieDomain> AddAsync(MovieDomain movieDomain);
        Task AddAsync(IEnumerable<MovieDomain> movieDomains);
    }
}
