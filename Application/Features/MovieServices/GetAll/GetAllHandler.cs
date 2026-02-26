using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.MovieServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<MovieResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public GetAllHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }
        public async Task<List<MovieResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var movies = await movieRepository.GetAllAsync(cancellationToken);
            var genres = await genreRepository.GetGenresDictionary();
            var directorsDict = await directorRepository.GetDirectorsDictionary();
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
