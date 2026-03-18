using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetListFromQuery(IQueryable<Movie> query, CancellationToken cancellationToken);
        IQueryable<Movie> AsQueryable();
        Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken);
        Task<Movie> AddAsync(Movie movieDomain, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<Movie> movieDomains, CancellationToken cancellationToken);
        Task<Movie?> FirstOrDefaultAsync(int movieId, CancellationToken cancellationToken);
        void DeleteMovie(Movie movie);
        Task AddListOfMovies(List<Movie> movieDomains, CancellationToken cancellationToken);
    }
}
