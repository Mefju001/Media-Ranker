using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence.Mapper;
using MediatR;
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

    }
}
