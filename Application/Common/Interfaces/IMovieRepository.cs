using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetListFromQuery(IQueryable<Movie> query, CancellationToken cancellationToken);
        IQueryable<Movie> AsQueryable();
        Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken);
        Task<Movie> AddAsync(Movie movieDomain);
        Task AddAsync(IEnumerable<Movie> movieDomains);
        Task<Movie?> FirstOrDefaultAsync(int movieId);
        Task DeleteMovie(Movie movieDomain);
        Task AddListOfMovies(List<Movie> movieDomains, CancellationToken cancellationToken);
    }
}
