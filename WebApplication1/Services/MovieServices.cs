using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class MovieServices : IMovieServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMovieBuilder movieBuilder;
        private readonly IReferenceDataService referenceDataService;
        public MovieServices(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IMovieBuilder builder, IMediator mediator)
        {
            this.referenceDataService = referenceDataService;
            this.movieBuilder = builder;
            this._unitOfWork = unitOfWork;
            this._mediator = mediator;
        }
        public async Task<List<MovieResponse>>AddListOfMovie(List<MovieRequest>requests)
        {
            if(requests is null)throw new ArgumentNullException(nameof(requests));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                List<Movie> movies = new List<Movie>();
                foreach (var request in requests)
                {
                    var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director);
                    var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
                    var movie = movieBuilder
                        .CreateNew(request.Title, request.Description)
                        .WithTechnicalDetails
                        (request.Duration,
                         request.Language,
                         request.IsCinemaRelease,
                         request.ReleaseDate)
                        .WithGenre(genre)
                        .WithDirector(director)
                        .Build();
                    movies.Add(movie);
                }
                await _unitOfWork.Movies.AddRangeAsync(movies);
                await _unitOfWork.CompleteAsync();
                var listOfResponses = movies.Select(MovieMapper.ToMovieResponse).ToList();
                await transaction.CommitAsync();
                return listOfResponses;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException();
            }
        }
        public async Task<MovieResponse> Upsert(int? movieId, MovieRequest movieRequest)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var director = await referenceDataService.GetOrCreateDirectorAsync(movieRequest.Director);
                var genre = await referenceDataService.GetOrCreateGenreAsync(movieRequest.Genre);
                Movie? movie= null;
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
                if (movie is null) throw new InvalidOperationException(nameof(movie));
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
            if (movie is null)
                return false;
            _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.CompleteAsync();
            return true;

        }
        public async Task<List<MovieResponse>> GetMoviesByCriteriaAsync(MovieQuery moviesQuery)
        {
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
            if(movie is null)
                return null;
            return MovieMapper.ToMovieResponse(movie);
        }
            
    }
}