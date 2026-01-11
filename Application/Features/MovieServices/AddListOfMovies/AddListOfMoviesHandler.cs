using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Application.Features.Movies.AddListOfMovies
{
    public class AddListOfGamesHandler : IRequestHandler<AddListOfGamesCommand, List<MovieResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieBuilder movieBuilder;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfGamesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IMovieBuilder builder)
        {
            this.referenceDataService = referenceDataService;
            this.movieBuilder = builder;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<MovieResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                List<Movie> movies = new List<Movie>();
                foreach (var request in requests.requests)
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
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
