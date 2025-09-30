using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Interfaces;
using WebApplication1.Models;
namespace WebApplication1.Services
{
    public class MovieServices(IUnitOfWork unitOfWork,IMovieBuilder builder) : IMovieServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMovieBuilder movieBuilder = builder;

        private async Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            var Director = await _unitOfWork.Directors.FirstOrDefaultAsync(d => d.name == directorRequest.Name && d.surname == directorRequest.Surname);
            if (Director is not null) return Director;
            Director = new Director { name = directorRequest.Name, surname = directorRequest.Surname };
            await _unitOfWork.Directors.AddAsync(Director);
            return Director;
        }
        private async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await _unitOfWork.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre != null) return genre;
            genre = new Genre { name = genreRequest.name };
            await _unitOfWork.Genres.AddAsync(genre);
            return genre;
        }
        public async Task<(int movieId, MovieResponse response)> Upsert(int? movieId, MovieRequest movieRequest)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var director = await GetOrCreateDirectorAsync(movieRequest.Director);
                var genre = await GetOrCreateGenreAsync(movieRequest.Genre);
                Movie? movie;
                if (movieId.HasValue)
                {
                    movie = await _unitOfWork.Movies
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
                        await _unitOfWork.CompleteAsync();
                        await transaction.CommitAsync();
                        return (movie.Id, MovieMapping.ToResponse(movie));
                    }
                }
                movie = movieBuilder
                    .CreateNew(movieRequest.Title, movieRequest.Description)
                    .WithOptionals(movieRequest.ReleaseDate, movieRequest.Language, movieRequest.Duration, movieRequest.IsCinemaRelease)
                    .WithGenre(genre)
                    .WithDirector(director)
                    .Build();
                await _unitOfWork.Movies.AddAsync(movie);
                await _unitOfWork.CompleteAsync();
                var response = MovieMapping.ToResponse(movie);
                await transaction.CommitAsync();
                return (movie.Id, response);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var movie = await _unitOfWork.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
                return false;
            _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.CompleteAsync();
            return true;

        }
        public async Task<List<MovieResponse>> GetSortAll(string sort)
        {
            sort = sort.ToLower();
            IQueryable<Movie> query = _unitOfWork.Movies.AsQueryable()
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User);
            if (!string.IsNullOrEmpty(sort) && sort.Equals("asc"))
            {
                query = query.OrderBy(m => m.title);
            }
            if (!string.IsNullOrEmpty(sort) && sort.Equals("desc"))
            {
                query = query.OrderByDescending(m => m.title);
            }
            var movies = await query.ToListAsync();
            var movieResponses = movies.Select(MovieMapping.ToResponse).ToList();
            return movieResponses;
        }
        public async Task<List<MovieResponse>> GetMoviesByAvrRating()
        {
            var moviesQuery = _unitOfWork.Movies.AsQueryable();
            var results = await moviesQuery
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
            return results.Select(x => MovieMapping.ToResponse(x.Movie)).ToList();
        }
        public async Task<List<MovieResponse>> GetMovies(string? name, string? genreName, string? directorName, int? movieId)
        {
            var query = _unitOfWork.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .Where(m => m.title.Contains(name));
            }
            if (!string.IsNullOrEmpty(genreName))
            {
                query = query
                    .Include(m => m.genre)
                    .Include(m => m.director)
                    .Include(m => m.Reviews)
                    .Where(m => m.genre.name.Contains(genreName));
            }
            if (!string.IsNullOrEmpty(directorName))
            {
                query = query
                    .Include(m => m.genre)
                    .Include(m => m.director)
                    .Include(m => m.Reviews)
                    .Where(m => m.director.name.Contains(directorName) || m.director.surname.Contains(directorName));
            }
            if (movieId.HasValue)
            {
                query = query
                    .Include(m => m.genre)
                    .Include(m => m.director)
                    .Include(m => m.Reviews)
                    .Where(m => m.Id == movieId.Value);
            }
            var movies = await query.ToListAsync();
            return movies.Select(MovieMapping.ToResponse).ToList();
        }
        public async Task<MovieResponse?> GetById(int id)
        {
            var movieQuery =  _unitOfWork.Movies.AsQueryable();
            var result = await movieQuery
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
                return null;
            var MovieResponse = MovieMapping.ToResponse(result);

            return MovieResponse;
        }
        public async Task<List<MovieResponse>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var MovieResponse = movies.Select(MovieMapping.ToResponse).ToList();
            return (MovieResponse);
        }
    }
}