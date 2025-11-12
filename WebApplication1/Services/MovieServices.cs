using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Notification;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;
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
                if(movie is null)throw new ArgumentNullException(nameof(movie));
                var response = MovieMapping.ToMovieResponse(movie);
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
        public async Task<List<MovieResponse>> GetMoviesByCriteriaAsync(MovieQuery moviesQuery)
        {
            var query = _unitOfWork.Movies.AsQueryable();
            var MovieResponses = await _mediator.Send(moviesQuery);
            return MovieResponses;
        }
        public async Task<List<MovieResponse>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var MovieResponse = movies.Select(MovieMapping.ToMovieResponse).ToList();
            return (MovieResponse);
        }
        public async Task<MediaStats> testForReviews()
        {
            await _mediator.Publish(new ReviewChangedNotification() { mediaId = 1 });
            await Task.Delay(TimeSpan.FromSeconds(5));
            var stats = await _unitOfWork.MediaStats.GetByIdAsync(1);
            return stats;
        }
        public async Task<MovieResponse?> GetById(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(movie, nameof(id));
            return MovieMapping.ToMovieResponse(movie);
        }
    }
}