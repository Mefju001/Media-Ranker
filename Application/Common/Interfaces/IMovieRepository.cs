using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<MovieDomain>> GetListFromQuery(IQueryable<MovieDomain> query, CancellationToken cancellationToken);
        IQueryable<MovieDomain> AsQueryable();
        Task<List<MovieDomain>> GetAllAsync(CancellationToken cancellationToken);
        Task<MovieDomain> AddAsync(MovieDomain movieDomain);
        Task AddAsync(IEnumerable<MovieDomain> movieDomains);
        Task<MovieDomain?> FirstOrDefaultAsync(int movieId);
        Task DeleteMovie(MovieDomain movieDomain);
        Task AddListOfMovies(List<MovieDomain> movieDomains, CancellationToken cancellationToken);
    }
}
