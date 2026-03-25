using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.MovieServices.AddListOfMovies;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.MovieServices.MovieUpsert;
using Domain.Entity;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace UnitTests
{
    [TestClass]
    public sealed class MovieServices
    {
        private GetMoviesByCriteriaHandler getMoviesByCriteriaHandler;
        private GetAllHandler getAllHandler;
        private MovieUpsertHandler movieUpsertHandler;
        private AddListOfMoviesHandler addListOfMoviesHandler;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IMovieRepository> mockMovieRepository;
        private Mock<IGenreRepository> mockGenreRepository;
        private Mock<IDirectorRepository> mockDirectorRepository;
        private Mock<IMediator> mediator;
        private Mock<IReferenceDataService> referenceMock;
        private Mock<IMovieSortAndFilterService> movieSortAndFilterService;
        private void SetupMocks()
        {
            mockMovieRepository = new Mock<IMovieRepository>();
            mockGenreRepository = new Mock<IGenreRepository>();
            mockDirectorRepository = new Mock<IDirectorRepository>();
            unitOfWork = new Mock<IUnitOfWork>();
            referenceMock = new Mock<IReferenceDataService>();
            mediator = new Mock<IMediator>();
            movieSortAndFilterService = new Mock<IMovieSortAndFilterService>();
        }
        [TestInitialize]
        public void Initialize()
        {
            SetupMocks();
            movieUpsertHandler = new MovieUpsertHandler(unitOfWork.Object, Mock.Of<ILogger<MovieUpsertHandler>>(), referenceMock.Object, mediator.Object, mockMovieRepository.Object);
            getAllHandler = new GetAllHandler(mockMovieRepository.Object, mockGenreRepository.Object, mockDirectorRepository.Object);
            addListOfMoviesHandler = new AddListOfMoviesHandler(Mock.Of<ILogger<AddListOfMoviesHandler>>(), mockMovieRepository.Object, referenceMock.Object, unitOfWork.Object, mediator.Object);
            getMoviesByCriteriaHandler = new GetMoviesByCriteriaHandler(movieSortAndFilterService.Object, mockMovieRepository.Object, mockGenreRepository.Object, mockDirectorRepository.Object);
        }

        [TestMethod]
        public void GetAll()
        {
            List<Movie> movies = new List<Movie>
            {
                Movie.Reconstruct(1,"Inception","A mind-bending thriller",new Language("polish"),new ReleaseDate(new DateTime(2010, 7, 16)),1,1,new Duration(TimeSpan.Zero),true, new MediaStats(6,2)),
                Movie.Reconstruct(2,"Interstellar","A journey through space and time",new Language("polish"),new ReleaseDate(new DateTime(2014, 11, 7)),2,2,new Duration(TimeSpan.Zero),false, new MediaStats(6,2))

            };
            var genresDictionary = new Dictionary<int, Genre>
            {
                {1, Genre.Reconstruct(1,"Sci-Fci")},
                {2, Genre.Reconstruct(2,"Fantasy")}
            };
            var directorDictionary = new Dictionary<int, Director>
            {
                {1, Director.Reconstruct(1,"Christopher","Nolan")},
                {2, Director.Reconstruct(2,"Steven","Spielberg")}
            };
            mockMovieRepository.Setup(u => u.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(movies);
            mockGenreRepository.Setup(g => g.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genresDictionary));
            mockDirectorRepository.Setup(d => d.GetDirectorsDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(directorDictionary));
            var query = new GetAllQuery();
            var results = getAllHandler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Result.Count());
            mockMovieRepository.Verify(u => u.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockGenreRepository.Verify(u => u.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
            mockDirectorRepository.Verify(u => u.GetDirectorsDictionary(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public void UpsertMovie_AddMovie()
        {
            var command = new UpsertMovieCommand(
                null,
                "Inception",
                "A mind-bending thriller",
                new GenreRequest("Sci-Fci"),
                new DirectorRequest("Christopher", "Nolan"),
                new DateTime(2010, 7, 16),
                "English",
                TimeSpan.FromMinutes(148),
                true
            );
            var movie = Movie.Reconstruct(1, "Inception", "A mind-bending thriller", new Language("polish"), new ReleaseDate(new DateTime(2010, 7, 16)), 1, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6, 2));
            referenceMock.Setup(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Genre.Reconstruct(1, "Sci-Fci"));
            referenceMock.Setup(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Director.Reconstruct(1, "Christopher", "Nolan"));
            mockMovieRepository.Setup(m => m.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
            var result = movieUpsertHandler.Handle(command, CancellationToken.None);
            referenceMock.Verify(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            referenceMock.Verify(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMovieRepository.Verify(m => m.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.Title, result.Result.Title);
        }
        [TestMethod]
        public void UpsertMovie_UpdateMovie()
        {
            var command = new UpsertMovieCommand(
                1,
                "Inception Updated",
                "A mind-bending thriller Updated",
                new GenreRequest("Sci-Fci"),
                new DirectorRequest("Christopher", "Nolan"),
                new DateTime(2010, 7, 16),
                "Polish",
                TimeSpan.FromMinutes(148),
                true
            );
            var existingMovie = Movie.Reconstruct(1, "Inception", "A mind-bending thriller", new Language("polish"), new ReleaseDate(new DateTime(2010, 7, 16)), 1, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6, 2));
            referenceMock.Setup(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Genre.Reconstruct(1, "Sci-Fci"));
            referenceMock.Setup(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Director.Reconstruct(1, "Christopher", "Nolan"));
            mockMovieRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingMovie);
            var result = movieUpsertHandler.Handle(command, CancellationToken.None);
            referenceMock.Verify(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            referenceMock.Verify(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMovieRepository.Verify(m => m.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.Title, result.Result.Title);
            Assert.AreEqual(command.Description, result.Result.Description);
            Assert.AreEqual(command.Language, result.Result.Language);
        }
        [TestMethod]
        public void AddListOfMovies()
        {
            var command = new AddListOfMoviesCommand(new List<MovieRequest>
            {
                new MovieRequest(
                    "Inception",
                    "A mind-bending thriller",
                    new GenreRequest("Sci-Fci"),
                    new DirectorRequest("Christopher", "Nolan"),
                    new DateTime(2010, 7, 16),
                    "English",
                    TimeSpan.FromMinutes(148),
                    true
                ),
                new MovieRequest
                (
                    "Interstellar",
                    "A journey through space and time",
                    new GenreRequest("Sci-Fci"),
                    new DirectorRequest("Christopher", "Nolan"),
                    new DateTime(2014, 11, 7),
                    "English",
                    TimeSpan.FromMinutes(169),
                    false
                )
            });
            var genreDictionary = new Dictionary<string, Genre>
            {
                {"Sci-Fci", Genre.Reconstruct(1,"Sci-Fci")},
                {"Fantasy",Genre.Reconstruct(2,"Fantasy")},
                { "Drama",Genre.Reconstruct(3,"Drama")   }
            };
            var directorDictionary = new Dictionary<(string, string), Director>
            {
                {("Christopher", "Nolan"), Director.Reconstruct(1,"Christopher","Nolan") },
                {("Steven", "Spielberg"), Director.Reconstruct(2,"Steven","Spielberg") }
            };
            referenceMock.Setup(g => g.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDictionary));
            referenceMock.Setup(d => d.EnsureDirectorsExistAsync(It.IsAny<List<DirectorRequest>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(directorDictionary));
            mockMovieRepository.Setup(m => m.AddListOfMovies(It.IsAny<List<Movie>>(), It.IsAny<CancellationToken>()));
            var results = addListOfMoviesHandler.Handle(command, CancellationToken.None);
            mockMovieRepository.Verify(m => m.AddListOfMovies(It.IsAny<List<Movie>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(results);
            Assert.HasCount(2, results.Result);
            Assert.AreEqual(command.movies[0].Title, results.Result[0].Title);
        }
        [TestMethod]
        public async Task GetMoviesByCriteria()
        {

            List<Movie> movies = new List<Movie>
            {
                Movie.Reconstruct(1, "Inception", "A mind-bending thriller", new Language("polish"), new ReleaseDate(new DateTime(2010, 7, 16)), 1, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6.2,2)),
                Movie.Reconstruct(2, "Interstellar", "A journey through space and time", new Language("polish"),  new ReleaseDate(new DateTime(2014, 11, 7)), 2, 2, new Duration(TimeSpan.Zero), false, new MediaStats(6.2,2)),
                Movie.Reconstruct(3, "The Dark Knight", "The Joker wreaks havoc on Gotham", new Language("polish"),  new ReleaseDate(new DateTime(2008, 7, 18)), 3, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6.2, 2)),
                Movie.Reconstruct(4, "The Matrix", "Reality is a simulation", new Language("polish"),  new ReleaseDate(new DateTime(1999, 3, 31)), 1, 2, new Duration(TimeSpan.Zero), true, new MediaStats(6.2, 2))
            };
            var genreDictionary = new Dictionary<int, Genre>
            {
                {1, Genre.Reconstruct(1,"Sci-Fci")},
                {2, Genre.Reconstruct(2,"Fantasy")},
                {3, Genre.Reconstruct(3,"Drama")   }
            };
            var directorDictionary = new Dictionary<int, Director>
            {
                {1, Director.Reconstruct(1,"Christopher","Nolan") },
                {2, Director.Reconstruct(2,"Steven","Spielberg") }
            };
            var queryFinish = movies.Where(m => m.GenreId == 1).ToList().BuildMock();
            movieSortAndFilterService.Setup(m => m.Handler(It.IsAny<GetMoviesByCriteriaQuery>())).Returns(queryFinish);
            mockGenreRepository.Setup(g => g.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDictionary));
            mockDirectorRepository.Setup(d => d.GetDirectorsDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(directorDictionary));
            mockMovieRepository.Setup(m => m.GetListFromQuery(It.IsAny<IQueryable<Movie>>(), It.IsAny<CancellationToken>())).ReturnsAsync(movies.Where(m => m.GenreId == 1).ToList());
            var query = new GetMoviesByCriteriaQuery();
            query.genreName = "Sci-Fci";
            query.IsDescending = true;
            query.SortByField = "Title";
            var results = await getMoviesByCriteriaHandler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(results);
            Assert.HasCount(2, results);
            Assert.AreEqual("The Matrix", results[1].Title);
            movieSortAndFilterService.Verify(m => m.Handler(It.IsAny<GetMoviesByCriteriaQuery>()), Times.Once);
            mockGenreRepository.Verify(g => g.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
            mockDirectorRepository.Verify(d => d.GetDirectorsDictionary(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
