using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Application.Notification;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Application.Features.Movies.MovieUpsert
{
    public class MovieUpsertHandler : IRequestHandler<UpsertMovieCommand, MovieResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMovieBuilder movieBuilder;
        private readonly IReferenceDataService referenceDataService;

        public MovieUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMovieBuilder movieBuilder, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.movieBuilder = movieBuilder;
            this.referenceDataService = referenceDataService;
        }

        public async Task<MovieResponse> Handle(UpsertMovieCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director);
                var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
                Movie? movie = null;
                if (request.id.HasValue)
                {
                    movie = await unitOfWork.Movies
                            .FirstOrDefaultAsync(m => m.Id == request.id.Value);
                    if (movie is not null)
                    {
                        MovieMapper.UpdateEntity(movie, request, director, genre);
                    }
                }
                else
                {
                    movie = movieBuilder
                        .CreateNew(request.Title, request.Description)
                        .WithTechnicalDetails
                        (request.Duration,
                         request.Language,
                         request.IsCinemaRelease,
                         request.ReleaseDate)
                        .WithGenre(genre)
                        .WithDirector(director)
                        .Build();
                    await unitOfWork.Movies.AddAsync(movie);
                }

                await unitOfWork.CompleteAsync();
                if (movie is null) throw new InvalidOperationException(nameof(movie));
                var response = MovieMapper.ToMovieResponse(movie);
                await transaction.CommitAsync();
                await _mediator.Publish(new LogNotification("Information", "Nowy film został dodany.", nameof(MovieUpsertHandler)));
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
