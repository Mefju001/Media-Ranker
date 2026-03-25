using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.GamesServices.AddListOfGames;
using Application.Mapper;
using Application.Notification;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.MovieServices.AddListOfMovies
{
    public class AddListOfMoviesHandler : IRequestHandler<AddListOfMoviesCommand, List<MovieResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        private readonly IMovieRepository movieRepository;
        private readonly ILogger<AddListOfMoviesHandler> logger;
        private readonly IMediator mediator;
        public AddListOfMoviesHandler(ILogger<AddListOfMoviesHandler> logger, IMovieRepository movieRepository, IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task<List<MovieResponse>> Handle(AddListOfMoviesCommand requests, CancellationToken cancellationToken)
        {
            if (requests.movies.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 movies at a time.");
            logger.LogInformation("Rozpoczęto dodawanie listy gier. Liczba elementów: {Count}", requests.movies.Count);
            var genreNames = requests.movies.Select(m => m.Genre.name).Distinct().ToList();
            var directors = requests.movies.Select(r => r.Director).Distinct().ToList();

            var dictionaryDirectors = await referenceDataService.EnsureDirectorsExistAsync(directors, cancellationToken);
            var dictionaryGenres = await referenceDataService.EnsureGenresExistAsync(genreNames, cancellationToken);

            var movies = requests.movies.Select(movieReq =>
            {
                var genre = dictionaryGenres[movieReq.Genre.name];
                var director = dictionaryDirectors[(movieReq.Director.Name, movieReq.Director.Surname)];
                return Movie.Create(movieReq.Title,
                    movieReq.Description,
                    new Language(movieReq.Language),
                    new ReleaseDate(movieReq.ReleaseDate),
                    genre.Id,
                    director.Id,
                    new Duration(movieReq.Duration),
                    movieReq.IsCinemaRelease);
            }).ToList();
            await movieRepository.AddListOfMovies(movies, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Pomyślnie dodano {Count} gier do bazy.", movies.Count);
            await mediator.Publish(new LogNotification("Information", "Nowa lista filmów została dodana.", nameof(AddListOfGamesHandler)));
            var directorById = dictionaryDirectors.Values.ToDictionary(d => d.Id);
            var genreById = dictionaryGenres.Values.ToDictionary(g => g.Id);
            return movies.Select(m =>
            {
                return MovieMapper.ToMovieResponse(m, genreById[m.GenreId], directorById[m.DirectorId]);
            }).ToList();
        }
    }
}
