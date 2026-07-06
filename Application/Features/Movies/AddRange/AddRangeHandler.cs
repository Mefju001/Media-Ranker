using Application.Common.Interfaces;
using Application.Features.Directors.Manager;
using Application.Features.Genres.GenreManager;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Movies.AddRange
{
    internal class AddRangeHandler : IRequestHandler<AddRangeCommand, List<Guid>>
    {
        private readonly IGenreManager genreHelperService;
        private readonly IDirectorManager directorHelperService;
        private readonly IMediaRepository<Movie> mediaRepository;
        public AddRangeHandler(IMediaRepository<Movie> mediaRepository, IGenreManager genreHelperService, IDirectorManager directorHelperService)
        {
            this.genreHelperService = genreHelperService;

            this.mediaRepository = mediaRepository;
            this.directorHelperService = directorHelperService;
        }
        public async Task<List<Guid>> Handle(AddRangeCommand requests, CancellationToken cancellationToken)
        {
            if (requests.movies == null || !requests.movies.Any())
                return [];
            if (requests.movies.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 movies at a time.");
            var genreNames = requests.movies.Select(m => m.Genre.name).Distinct().ToList();
            var directorNames = requests.movies.Select(r => r.Director).Distinct().ToList();
            var listDirectors = await directorHelperService.GetOrCreateBatchAsync(directorNames, cancellationToken);
            var listGenres = await genreHelperService.GetOrCreateBatchAsync(genreNames, cancellationToken);
            var dictionaryDirectors = listDirectors.ToDictionary(d => (d.Name, d.Surname));
            var dictionaryGenres = listGenres.ToDictionary(g => g.Name);
            var movies = requests.movies.Select(movieReq =>
            {
                var genre = dictionaryGenres[movieReq.Genre.name];
                var director = dictionaryDirectors[(movieReq.Director.Name, movieReq.Director.Surname)];
                return Movie.Create(movieReq.Title,
                    movieReq.Description,
                    movieReq.Language,
                    new ReleaseDate(movieReq.ReleaseDate),
                    genre.id,
                    director.id,
                    new Duration(movieReq.Duration),
                    movieReq.DistributionType,
                    movieReq.Status
                );
            }).ToList();
            await mediaRepository.AddRangeAsync(movies, cancellationToken);
            return movies.Select(m => m.Id).ToList();
        }
    }
}
