using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;

namespace Application.Features.MovieServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<MovieResponse>>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public GetAllHandler(IMediaRepository<Movie> mediaRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }
        public async Task<List<MovieResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var movies = await mediaRepository.GetAllAsync(cancellationToken);
            // Selective fetching of genres and directors to avoid N+1 problem in future
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var directorsDict = await directorRepository.GetDirectorsDictionary(cancellationToken);

            var movieResponse = movies.Select(m =>
            {
                genres.TryGetValue(m.GenreId, out var genre);
                directorsDict.TryGetValue(m.DirectorId, out var director);
                return MovieMapper.ToMovieResponse(m, genre!, director!);
            }).ToList();
            return movieResponse;
        }
    }
}
