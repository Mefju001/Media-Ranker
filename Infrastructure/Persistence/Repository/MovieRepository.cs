using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext context;
        public MovieRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Movie>> GetListFromQuery(IQueryable<Movie> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
        }
        public async Task<Movie> AddAsync(Movie movieDomain, CancellationToken cancellationToken)
        {
            var movie = await context.Movies.AddAsync(movieDomain, cancellationToken);
            return movie.Entity;
        }

        public async Task AddAsync(IEnumerable<Movie> movieDomains, CancellationToken cancellationToken)
        {
            await context.Movies.AddRangeAsync(movieDomains, cancellationToken);
        }

        public async Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken)
        {
            var movies = await context.Movies.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
            return movies;
        }
        public async Task<Movie?> FirstOrDefaultAsync(int movieId, CancellationToken cancellationToken)
        {
            return await context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == movieId, cancellationToken);
        }

        public IQueryable<Movie> AsQueryable()
        {
            return context.Movies
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
        }

        public void DeleteMovie(Movie movie)
        {
            context.Movies.Remove(movie);
        }

        public async Task AddListOfMovies(List<Movie> movieDomains, CancellationToken cancellationToken)
        {
            await context.AddRangeAsync(movieDomains, cancellationToken);
        }
    }
}
