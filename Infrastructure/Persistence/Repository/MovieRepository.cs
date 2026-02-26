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
            return await query.ToListAsync(cancellationToken);
        }
        public async Task<Movie> AddAsync(Movie movieDomain)
        {
            var movie = await context.Movies.AddAsync(movieDomain);
            return movie.Entity;
        }

        public async Task AddAsync(IEnumerable<Movie> movieDomains)
        {
            await context.Movies.AddRangeAsync(movieDomains);
        }

        public async Task<List<Movie>> GetAllAsync(CancellationToken cancellationToken)
        {
            var movies = await context.Movies.ToListAsync();
            return movies;
        }
        public async Task<Movie?> FirstOrDefaultAsync(int movieId)
        {
            return await context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        }

        public IQueryable<Movie> AsQueryable()
        {
            return context.Movies
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task DeleteMovie(Movie movieDomain)
        {
            context.Movies.Remove(movieDomain);
        }

        public async Task AddListOfMovies(List<Movie> movieDomains, CancellationToken cancellationToken)
        {
            await context.AddRangeAsync(movieDomains, cancellationToken);
        }
    }
}
