using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
namespace WebApplication1.Services.Impl
{
    public class MovieServices(AppDbContext context, IMapper mapper) : IMovieServices
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper mapper = mapper;

        private async Task<Director>GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            var Director = await _context.Directors.FirstOrDefaultAsync(d => d.name == directorRequest.Name && d.surname == directorRequest.Surname);
            if(Director is not null) return Director;
            Director = new Director { name = directorRequest.Name, surname = directorRequest.Surname };
            _context.Directors.Add(Director);
            return Director;
        }
        private async Task<Genre>GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre =  await _context.Genres.FirstOrDefaultAsync(g=>g.name==genreRequest.name);
            if(genre is not null)return genre;
            genre = new Genre { name = genreRequest.name };
            _context.Genres.Add(genre);
            return genre;
        }
        public async Task<(int movieId, MovieResponse response)> Upsert(int? movieId,MovieRequest movieRequest)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var director = await GetOrCreateDirectorAsync(movieRequest.Director);
                var genre = await GetOrCreateGenreAsync(movieRequest.Genre);
                Movie? movie;
                if (movieId is not null)
                {
                    movie = await _context.Movies
                            .Include(m => m.director)
                            .Include(m => m.genre)
                            .FirstOrDefaultAsync(m => m.Id == movieId.Value);
                    if (movie is not null)
                    {
                        movie.title = movieRequest.Title;
                        movie.description = movieRequest.Description;
                        movie.director = director;
                        movie.genre = genre;
                        movie.ReleaseDate = movieRequest.ReleaseDate;
                        movie.Language = movieRequest.Language;
                        movie.IsCinemaRelease = movieRequest.IsCinemaRelease;
                        movie.Duration = movieRequest.Duration;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return (movie.Id, MovieMapping.ToResponse(movie));
                    }
                }
                movie = new Movie
                {
                    title = movieRequest.Title,
                    description = movieRequest.Description,
                    director = director,
                    genre = genre
                };
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                var response = MovieMapping.ToResponse(movie);
                await transaction.CommitAsync();
                return (movie.Id, response);
            }
            catch {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<MovieResponse>>GetSortAll(string  sort)
        {
            sort = sort.ToLower();
            var query = _context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .AsQueryable();
            if (!string.IsNullOrEmpty(sort) && sort.Equals("asc"))
            {
                query = query.OrderBy(m => m.title);
            }
            if(!string.IsNullOrEmpty(sort) && sort.Equals("desc"))
            {
                query = query.OrderByDescending(m => m.title);
            }
            var movies = await query.ToListAsync();
            var map = mapper.Map<List<MovieResponse>>(movies);
            return map.ToList();
        }
        public async Task<List<MovieResponse>> GetMoviesByAvrRating()
        {
            var movies = await _context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .Select(m => new
                {
                    Movie = m,
                    avarage = m.Reviews.Average(r => (double?)r.Rating) ?? 0
                })
                .OrderByDescending(x => x.avarage)
                .ToListAsync();
            return movies.Select(x=>MovieMapping.ToResponse(x.Movie)).ToList();
        }
        public async Task<List<MovieResponse>> GetMovies(string? name, string? genreName, string? directorName, int? movieId)
        {
            var query = _context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.title.Contains(name));
            }
            if (!string.IsNullOrEmpty(genreName))
            {
                query = query.Where(m=>m.genre.name.Contains(genreName));
            }
            if (!string.IsNullOrEmpty(directorName))
            {
                query = query.Where(m=>m.director.name.Contains(directorName)||m.director.surname.Contains(directorName));
            }
            if (movieId.HasValue)
            {
                query = query.Where(m=>m.Id == movieId.Value);
            }
            var movies = await query.ToListAsync();
            return movies.Select(MovieMapping.ToResponse).ToList();
        }
        public async Task<MovieResponse?> GetById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return null;
            return MovieMapping.ToResponse(movie);
        }
        public async Task<List<MovieResponse>> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .ToListAsync();



            var MovieResponse = movies.Select(MovieMapping.ToResponse).ToList();
            return (MovieResponse);
        }
    }
}