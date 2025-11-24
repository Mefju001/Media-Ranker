using MediatR;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Notification;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Services
{
    public class MovieServices(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IMovieBuilder builder, IMediator mediator) : IMovieServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMediator _mediator = mediator;
        private readonly IMovieBuilder movieBuilder = builder;
        private readonly IReferenceDataService referenceDataService = referenceDataService;

        public async Task<MovieResponse> Upsert(int? movieId, MovieRequest movieRequest)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var director = await referenceDataService.GetOrCreateDirectorAsync(movieRequest.Director);
                var genre = await referenceDataService.GetOrCreateGenreAsync(movieRequest.Genre);
                Movie? movie;
                if (movieId.HasValue)
                {
                    movie = await _unitOfWork.Movies
                            .FirstOrDefaultAsync(m => m.Id == movieId.Value);
                    if (movie is not null)
                    {
                        MovieMapper.UpdateEntity(movie, movieRequest, director, genre);
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
                if (movie is null) throw new ArgumentNullException(nameof(movie));
                var response = MovieMapper.ToMovieResponse(movie);
                await transaction.CommitAsync();
                await _mediator.Publish(new LogNotification("Information", "Nowy film zosta³ dodany.", nameof(MovieServices)));
                return response;
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
            var MovieResponse = movies.Select(MovieMapper.ToMovieResponse).ToList();
            return (MovieResponse);
        }
        public async Task<MovieResponse?> GetById(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(movie, nameof(id));
            return MovieMapper.ToMovieResponse(movie);
        }
    }
}