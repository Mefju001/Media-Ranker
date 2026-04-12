using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.MovieServices.MovieUpsert
{
    public class MovieUpsertHandler : IRequestHandler<UpsertMovieCommand, MovieResponse>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IMediator mediator;
        private readonly IGenreHelperService genreHelperService;
        private readonly IDirectorHelperService directorHelperService;

        public MovieUpsertHandler(IDirectorHelperService directorHelperService, IGenreHelperService genreHelperService, IMediator mediator, IMediaRepository<Movie> mediaRepository)
        {
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.directorHelperService = directorHelperService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MovieResponse> Handle(UpsertMovieCommand request, CancellationToken cancellationToken)
        {
            var director = await directorHelperService.GetOrCreateDirectorAsync(request.Director, cancellationToken);
            var genre = await genreHelperService.GetOrCreateGenreAsync(request.Genre, cancellationToken);
            var isNew = false;
            Movie? movie = null;
            if (request.id.HasValue)
            {
                movie = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken);
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
            }
            else
            {
                isNew = true;
                movie = Movie.Create(request.Title,
                            request.Description,
                            new Language(request.Language),
                            new ReleaseDate(request.ReleaseDate!.Value),
                            genre.Id,
                            director.Id,
                            new Duration(request.Duration),
                            request.IsCinemaRelease);
                movie = await mediaRepository.AddAsync(movie, cancellationToken);
            }
            var action = isNew ? "dodana" : "zaktualizowana";
            if (movie is null) throw new InvalidOperationException(nameof(movie));
            await mediator.Publish(new LogNotification("Information", $"Film został {action}.", nameof(MovieUpsertHandler)));
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }

    }
}
