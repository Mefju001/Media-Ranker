using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Exceptions;
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

        public async Task<MovieDomain> AddAsync(MovieDomain movieDomain)
        {
            var movie = await context.Movies.AddAsync(movieDomain);
            return movie.Entity;
        }

        public async Task AddAsync(IEnumerable<MovieDomain> movieDomains)
        {
            await context.Movies.AddRangeAsync(movieDomains);
        }

        public async Task<IEnumerable<MovieDomain>> GetAllAsync()
        {
            var movies = await context.Movies.ToListAsync();
            return movies;
        }
        public async Task<MovieDomain?> FirstOrDefaultAsync(int movieId)
        {
            return await context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        }

        public IQueryable<MovieDomain> AsQueryable()
        {
            return context.Movies
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task DeleteMovie(MovieDomain movieDomain)
        {
            context.Movies.Remove(movieDomain);
        }

        public async Task AddListOfMovies(List<MovieDomain> movieDomains, CancellationToken cancellationToken)
        {
            await context.AddRangeAsync(movieDomains,cancellationToken);
        }
    }
}
