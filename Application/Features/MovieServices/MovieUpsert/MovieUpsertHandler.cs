using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Entity;
using Domain.Value_Object;
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
            var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director);
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
            Movie? movie;
            if (request.id.HasValue)
            {
                movie = await unitOfWork.MovieRepository.FirstOrDefaultAsync(request.id.Value);
                if (movie is not null)
                {
                    movie.Update(
                        request.Title,
                        request.Description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate.Value),
                        genre.Id,
                        director.Id,
                        new Duration(request.Duration),
                        request.IsCinemaRelease
                        );
                }
            }
            else
            {
                movie = Movie.Create(request.Title,
                                            request.Description,
                                            new Language(request.Language),
                                            new ReleaseDate(request.ReleaseDate.Value),
                                            genre.Id,
                                            director.Id,
                                            new Duration(request.Duration),
                                            request.IsCinemaRelease);
                movie = await unitOfWork.MovieRepository.AddAsync(movie);
            }

            await unitOfWork.CompleteAsync();
            if (movie is null) throw new InvalidOperationException(nameof(movie));
            var response = MovieMapper.ToMovieResponse(movie,genre,director);
            await _mediator.Publish(new LogNotification("Information", "Nowy film został dodany.", nameof(MovieUpsertHandler)));
            return response;
        }

    }
}
