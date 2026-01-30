using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.MovieServices.AddListOfMovies
{
    public class AddListOfMoviesHandler : IRequestHandler<AddListOfMoviesCommand, List<MovieResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfMoviesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork)
        {
            this.referenceDataService = referenceDataService;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<MovieResponse>> Handle(AddListOfMoviesCommand requests, CancellationToken cancellationToken)
        {
            var genreNames = requests.movies.Select(m => m.Genre.name).Distinct().ToList();
            var directors = requests.movies.Select(r => r.Director).ToList();
            var dictionaryDirectors = await referenceDataService.EnsureDirectorsExistAsync(directors);
            var dictionaryNames = await referenceDataService.EnsureGenresExistAsync(genreNames);
            var movies = requests.movies.Select(movieReq =>
            {
                var genre = dictionaryNames[movieReq.Genre.name];
                var director = dictionaryDirectors[(movieReq.Director.Name, movieReq.Director.Surname)];
                return MovieDomain.Create(movieReq.Title, movieReq.Description, movieReq.Language,
                                                  movieReq.ReleaseDate, genre, director,
                                                  movieReq.Duration, movieReq.IsCinemaRelease);
            }).ToList();
            await _unitOfWork.MovieRepository.AddListOfMovies(movies, cancellationToken);
            await _unitOfWork.CompleteAsync();
            return movies.Select(MovieMapper.ToMovieResponse).ToList();
        }
    }
}
