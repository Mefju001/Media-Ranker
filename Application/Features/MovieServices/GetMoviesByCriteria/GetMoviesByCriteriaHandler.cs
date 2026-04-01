using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IMovieSortAndFilterService SortAndFilterService;
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public GetMoviesByCriteriaHandler(IMovieSortAndFilterService sortAndFilterService, IMediaRepository<Movie> mediaRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            SortAndFilterService = sortAndFilterService;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = SortAndFilterService.Handler(request);
            var movies = await mediaRepository.FromAsQueryableToList(query, cancellationToken);
            var genresDictionary = await genreRepository.GetGenresDictionary(cancellationToken);
            var directorsDictionary = await directorRepository.GetDirectorsDictionary(cancellationToken);
            var responses = movies.Select(m =>
            {
                genresDictionary.TryGetValue(m.GenreId, out var genreDomain);
                directorsDictionary.TryGetValue(m.DirectorId, out var directorDomain);
                return MovieMapper.ToMovieResponse(m, genreDomain!, directorDomain!);
            }).ToList();
            return responses;
        }
    }
}
