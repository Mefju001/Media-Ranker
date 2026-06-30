using Application.Common.Interfaces;
using Application.Features.Directors.Manager;
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
        private readonly IGenreManager genreHelperService;
        private readonly IDirectorManager directorHelperService;

        public UpsertHandler(IDirectorManager directorHelperService, IGenreManager genreHelperService, IMediaRepository<Movie> mediaRepository)
        {
            this.genreHelperService = genreHelperService;
            this.directorHelperService = directorHelperService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MovieResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var director = await directorHelperService.GetOrCreateAsync(request.Director, cancellationToken);
            var genre = await genreHelperService.GetOrCreateAsync(request.Genre, cancellationToken);
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
                    request.Language,
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.id,
                    director.id,
                    new Duration(request.Duration),
                    request.IsCinemaRelease
                );
            }
            else
            {
                movie = Movie.Create(request.Title,
                            request.Description,
                            request.Language,
                            new ReleaseDate(request.ReleaseDate!.Value),
                            genre.id,
                            director.id,
                            new Duration(request.Duration),
                            request.IsCinemaRelease);
                movie = await mediaRepository.AddAsync(movie, cancellationToken);
            }
            if (movie is null) throw new InvalidOperationException(nameof(movie));
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }

    }
}
