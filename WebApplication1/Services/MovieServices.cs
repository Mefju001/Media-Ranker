using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using WebApplication1.Strategy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace WebApplication1.Services
{
    public class MovieServices(IUnitOfWork unitOfWork, IMovieBuilder builder, IMediator mediator, MovieQueryHandler handler) : IMovieServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMediator _mediator = mediator;
        private readonly IMovieBuilder movieBuilder = builder;
        private readonly MovieQueryHandler handler = handler;

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
                        MovieMapping.UpdateEntity(movie, movieRequest, director, genre);
                    }
                }
                else
                {
                    movie = movieBuilder
                        .CreateNew(movieRequest.Title, movieRequest.Description)
                        .WithTechnicalDetails
                        (movieRequest.Duration,
                         movieRequest.Language,
                         movieRequest.IsCinemaRelease,
                         movieRequest.ReleaseDate)
                        .WithGenre(genre)
                        .WithDirector(director)
                        .Build();
                    await _unitOfWork.Movies.AddAsync(movie);
                }

                await _unitOfWork.CompleteAsync();
                var response = MovieMapping.ToResponse(movie);
                await transaction.CommitAsync();
                await _mediator.Publish(new LogNotification("Information", "Nowy film zosta³ dodany.", nameof(MovieServices)));
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
        public async Task<List<MovieResponse>> GetSortAll(string sortByField, string sortByDirection)
        {
            sortByDirection = sortByDirection.ToLower();
            sortByField = sortByField.ToLower();
            IQueryable<Movie> query = _unitOfWork.Movies.AsQueryable()
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User);
            if(!string.IsNullOrEmpty(sortByDirection)||!string.IsNullOrEmpty(sortByField))
            { 
                bool isDesceding = sortByDirection.Equals("desc",StringComparison.OrdinalIgnoreCase);
                query = handler.Handle(sortByField, isDesceding);
            }

            var movies = await query.ToListAsync();
            var movieResponses = movies.Select(MovieMapping.ToResponse).ToList();
            return movieResponses;
        }
        public async Task<List<MovieAVGResponse>> GetMoviesByAvrRating(string sortByDirection)
        {
            var moviesQuery = _unitOfWork.Movies.AsQueryable();
            /*var results = moviesQuery
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews);
            .Select(m => new
            {
                Movie = m,
                avarage = m.Reviews.Average(r => (double?)r.Rating) ?? 0
            })
            .OrderByDescending(x => x.avarage)
            .ToListAsync();*/
            IQueryable<Movie> query = _unitOfWork.Movies.AsQueryable()
            .Include(m => m.genre)
            .Include(m => m.director)
            .Include(m => m.Reviews)
                .ThenInclude(r => r.User);
            if (!string.IsNullOrEmpty(sortByDirection))
            {
                bool isDesceding = sortByDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
                query = handler.Handle(null,isDesceding);
            }
            return query.Select(x => MovieAVGMapping.ToResponse(x.,x.avarage)).ToList();
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
            var movieQuery = _unitOfWork.Movies.AsQueryable();
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