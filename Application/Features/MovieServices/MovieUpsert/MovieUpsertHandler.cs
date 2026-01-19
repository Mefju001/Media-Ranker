using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.MovieUpsert
{
    public class MovieUpsertHandler : IRequestHandler<UpsertMovieCommand, MovieResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IReferenceDataService referenceDataService;

        public MovieUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.referenceDataService = referenceDataService;
        }

        public async Task<MovieResponse> Handle(UpsertMovieCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director);
                var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
                MovieDomain? movie = null;
                if (request.id.HasValue)
                {
                    movie = await unitOfWork.MovieRepository.FirstOrDefaultAsync(request.id.Value);
                    if (movie is not null)
                    {
                        MovieDomain.Update(
                            request.Title,
                            request.Description,
                            request.Language,
                            request.ReleaseDate!.Value,
                            genre.Id,
                            director.Id,
                            request.Duration,
                            request.IsCinemaRelease,
                            movie);
                    }
                }
                else
                {
                    movie = MovieDomain.Create(request.Title,
                                               request.Description,
                                               request.Language,
                                               request.ReleaseDate ?? DateTime.Now,
                                               genre.Id,
                                               director.Id,
                                               request.Duration,
                                               request.IsCinemaRelease);
                    await unitOfWork.MovieRepository.AddAsync(movie);
                }

                await unitOfWork.CompleteAsync();
                if (movie is null) throw new InvalidOperationException(nameof(movie));
                var response = MovieMapper.ToMovieResponse(movie,genre,director);
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
