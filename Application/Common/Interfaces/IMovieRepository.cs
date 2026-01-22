using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        IQueryable<MovieDomain> AsQueryable();
        Task<IEnumerable<MovieDomain>> GetAllAsync();
        Task<MovieDomain> AddAsync(MovieDomain movieDomain);
        Task AddAsync(IEnumerable<MovieDomain> movieDomains);
        Task<MovieDomain?> FirstOrDefaultAsync(int movieId);
        Task DeleteMovie(MovieDomain movieDomain);
        Task AddListOfMovies(List<MovieDomain>movieDomains, CancellationToken cancellationToken);
    }
}
