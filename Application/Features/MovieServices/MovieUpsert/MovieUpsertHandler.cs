using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.MovieServices.MovieUpsert
{
    public class MovieUpsertHandler : IRequestHandler<UpsertMovieCommand, MovieResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediaRepository mediaRepository;
        private readonly IMediator mediator;
        private readonly IReferenceDataService referenceDataService;
        private readonly ILogger<MovieUpsertHandler> logger;

        public MovieUpsertHandler(IUnitOfWork unitOfWork, ILogger<MovieUpsertHandler> logger, IReferenceDataService referenceDataService, IMediator mediator, IMediaRepository mediaRepository)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.referenceDataService = referenceDataService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MovieResponse> Handle(UpsertMovieCommand request, CancellationToken cancellationToken)
        {
            var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director, cancellationToken);
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre, cancellationToken);
            Movie? movie = null;
            if (request.id.HasValue)
            {
                movie = await mediaRepository.GetByIdAsync<Movie>(request.id.Value, cancellationToken);
            }
            if (movie is not null)
            {
                movie.Update(
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.Id,
                    director.Id,
                    new Duration(request.Duration),
                    request.IsCinemaRelease
                );
                logger.LogInformation("Updating movie with id {MovieId}", movie.Id);
            }
            else
            {
                movie = Movie.Create(request.Title,
                            request.Description,
                            new Language(request.Language),
                            new ReleaseDate(request.ReleaseDate!.Value),
                            genre.Id,
                            director.Id,
                            new Duration(request.Duration),
                            request.IsCinemaRelease);
                movie = await mediaRepository.AddAsync(movie, cancellationToken);
                logger.LogInformation("Creating new movie with title {movieTitle}", movie.Title);
            }

            await unitOfWork.CompleteAsync(cancellationToken);
            if (movie is null) throw new InvalidOperationException(nameof(movie));
            var response = MovieMapper.ToMovieResponse(movie, genre, director);
            logger.LogInformation("Movie with id {MovieId} has been upserted successfully", movie.Id);
            await mediator.Publish(new LogNotification("Information", "Nowy film został dodany.", nameof(MovieUpsertHandler)));
            return response;
        }

    }
}
