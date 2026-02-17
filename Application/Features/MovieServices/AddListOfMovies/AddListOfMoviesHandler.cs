using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Value_Object;
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
            var directors = requests.movies.Select(r => r.Director).Distinct().ToList();
            var dictionaryDirectors = await referenceDataService.EnsureDirectorsExistAsync(directors);
            var dictionaryGenres = await referenceDataService.EnsureGenresExistAsync(genreNames);
            var movies = requests.movies.Select(movieReq =>
            {
                var genre = dictionaryGenres[movieReq.Genre.name];
                var director = dictionaryDirectors[(movieReq.Director.Name, movieReq.Director.Surname)];
                var language = new Language(movieReq.Language);
                var releaseDate = new ReleaseDate(movieReq.ReleaseDate);
                var duration = new Duration(movieReq.Duration);
                return Movie.Create(movieReq.Title, movieReq.Description, language,
                                                  releaseDate, genre.Id, director.Id,
                                                  duration, movieReq.IsCinemaRelease);
            }).ToList();
            await _unitOfWork.MovieRepository.AddListOfMovies(movies, cancellationToken);
            await _unitOfWork.CompleteAsync();
            return movies.Select(m=>
            {
                var directorDomain = dictionaryDirectors.Values.ToDictionary(d => d.Id);
                var genreDomain = dictionaryGenres.Values.ToDictionary(g => g.Id);
                return MovieMapper.ToMovieResponse(m, genreDomain[m.GenreId], directorDomain[m.DirectorId]);
            }).ToList();
        }
    }
}
