using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Directors_MOZE_EDYCJA.Manager;
using Application.Features.Genres.GenreManager;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Movies.Upsert
{
    internal class UpsertHandler : IRequestHandler<UpsertCommand, MovieResponse>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IMediator mediator;
        private readonly IGenreManager genreHelperService;
        private readonly IDirectorManager directorHelperService;

        public UpsertHandler(IDirectorManager directorHelperService, IGenreManager genreHelperService, IMediator mediator, IMediaRepository<Movie> mediaRepository)
        {
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.directorHelperService = directorHelperService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MovieResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var director = await directorHelperService.GetOrCreateAsync(request.Director, cancellationToken);
            var genre = await genreHelperService.GetOrCreateAsync(request.Genre, cancellationToken);
            var isNew = false;
            Movie? movie = null;
            if (request.id.HasValue)
            {
                movie = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken) ?? throw new NotFoundException($"Movie {request.id} not found");
            }
            if (movie is not null)
            {
                movie.Update(
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.id,
                    director.id,
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
                            genre.id,
                            director.id,
                            new Duration(request.Duration),
                            request.IsCinemaRelease);
                movie = await mediaRepository.AddAsync(movie, cancellationToken);
            }
            var action = isNew ? "dodana" : "zaktualizowany";
            if (movie is null) throw new InvalidOperationException(nameof(movie));
            await mediator.Publish(new LogNotification("Information", $"Film został {action}.", nameof(UpsertHandler)));
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }

    }
}
