using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.TvSeriesServices.AddListOfTvSeries;
using Application.Features.TvSeriesServices.GetAll;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using Domain.Entity;
using Domain.Enums;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace UnitTests
{
    [TestClass]
    public sealed class TvSeriesTest
    {
        private Mock<ITvSeriesSortAndFilterService> sortAndFilterService;
        private Mock<IMediator> mediator;
        private Mock<IReferenceDataService> referenceDataService;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITvSeriesRepository> tvSeriesRepository;
        private Mock<IGenreRepository> genreRepository;

        private GetAllHandler handler;
        private TvSeriesUpsertHandler upsertHandler;
        private AddListOfTvSeriesHandler addListOfTvSeriesHandler;
        private GetTvSeriesByCriteriaHandler criteriaHandler;
        private void SetupMocks()
        {
            referenceDataService = new Mock<IReferenceDataService>();
            mediator = new Mock<IMediator>();
            unitOfWork = new Mock<IUnitOfWork>();
            sortAndFilterService = new Mock<ITvSeriesSortAndFilterService>();
            tvSeriesRepository = new Mock<ITvSeriesRepository>();
            genreRepository = new Mock<IGenreRepository>();
        }
        [TestInitialize]
        public void Setup()
        {
            SetupMocks();
            handler = new GetAllHandler(tvSeriesRepository.Object, genreRepository.Object);
            upsertHandler = new TvSeriesUpsertHandler(unitOfWork.Object, referenceDataService.Object, mediator.Object, tvSeriesRepository.Object, Mock.Of<ILogger<TvSeriesUpsertHandler>>());
            criteriaHandler = new GetTvSeriesByCriteriaHandler(sortAndFilterService.Object, tvSeriesRepository.Object, genreRepository.Object);
            addListOfTvSeriesHandler = new AddListOfTvSeriesHandler(mediator.Object, referenceDataService.Object, Mock.Of<ILogger<AddListOfTvSeriesHandler>>(), unitOfWork.Object, tvSeriesRepository.Object);
        }
        [TestMethod]
        public async Task GetAllTvSeries_ReturnsTvSeriesList()
        {
            var TvSeriesList = new List<TvSeries>
            {
                TvSeries.Reconstruct(1, "Breaking Bad", "A high school chemistry teacher turned methamphetamine producer.", new Language("English"), new ReleaseDate(new DateTime(2008, 1, 20)), 1, 5, 62, "AMC", EStatus.Completed, new MediaStats(7,1)),
                TvSeries.Reconstruct(2, "Game of Thrones", "Nine noble families fight for control over the lands of Westeros.",new Language("English"), new ReleaseDate(new DateTime(2011, 4, 17)), 2, 8, 73, "HBO", EStatus.Completed, new MediaStats(7,1))
            };
            tvSeriesRepository.Setup(u => u.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(TvSeriesList);
            var genresDictionary = new Dictionary<int, Genre>
            {
                { 1, Genre.Reconstruct(1, "Crime" )  },
                { 2, Genre.Reconstruct(2, "Fantasy" ) }
            };
            genreRepository.Setup(u => u.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genresDictionary));
            var result = await handler.Handle(new GetAllTvSeriesQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            tvSeriesRepository.Verify(u => u.GetAll(It.IsAny<CancellationToken>()), Times.Once);
            genreRepository.Verify(u => u.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task AddTvSeries()
        {
            var genre = Genre.Reconstruct(1, "Crime");
            referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genre));
            var command = new UpsertTvSeriesCommand(null,
                "Breaking Bad",
                "A high school chemistry teacher turned methamphetamine producer.",
                new GenreRequest("Crime"),
                new DateTime(2008, 1, 20),
                "English",
                5,
                62,
                "AMC",
                EStatus.Completed
            );
            var createdTvSeries = TvSeries.Reconstruct(1, command.title, command.description, new Language(command.Language), new ReleaseDate(command.ReleaseDate), genre.Id, command.Seasons, command.Episodes, command.Network, command.Status, new MediaStats(0, 0));
            tvSeriesRepository.Setup(u => u.AddTvSeriesAsync(It.IsAny<TvSeries>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(createdTvSeries));
            var result = await upsertHandler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.title, result.Title);
            tvSeriesRepository.Verify(u => u.AddTvSeriesAsync(It.IsAny<TvSeries>(), It.IsAny<CancellationToken>()), Times.Once);
            referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task UpdateTvSeries()
        {
            var genre = Genre.Reconstruct(1, "Crime");
            referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genre));
            var existingTvSeries = TvSeries.Reconstruct(1, "Breaking Bad", "A high school chemistry teacher turned methamphetamine producer.", new Language("English"), new ReleaseDate(new DateTime(2008, 1, 20)), 1, 5, 62, "AMC", EStatus.Completed, new MediaStats(7, 1));
            tvSeriesRepository.Setup(u => u.GetTvSeriesById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingTvSeries);
            var command = new UpsertTvSeriesCommand(1,
                "Breaking Bad Updated",
                "Updated description.",
                new GenreRequest("Crime"),
                new DateTime(2008, 1, 20),
                "English",
                5,
                62,
                "AMC",
                EStatus.Completed
            );
            var result = await upsertHandler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.title, result.Title);
            Assert.AreEqual(command.description, result.Description);
            tvSeriesRepository.Verify(u => u.GetTvSeriesById(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task AddListOfTvSeries()
        {
            var genre = Genre.Reconstruct(1, "Crime");
            referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genre));
            var commands = new AddListOfTvSeriesCommand(
                new List<TvSeriesRequest>
                {
                new TvSeriesRequest(
                    "Breaking Bad",
                    "A high school chemistry teacher turned methamphetamine producer.",
                    new GenreRequest("Crime"),
                    new DateTime(2008, 1, 20),
                    "English",
                    5,
                    62,
                    "AMC",
                    EStatus.Completed
                ),
                new TvSeriesRequest(
                    "Game of Thrones",
                    "Nine noble families fight for control over the lands of Westeros.",
                    new GenreRequest("Fantasy"),
                    new DateTime(2011, 4, 17),
                    "English",
                    8,
                    73,
                    "HBO",
                    EStatus.Completed
                )
                }
            );
            var listOFGenres = new Dictionary<string, Genre>
            {
                { "Crime",Genre.Reconstruct(1, "Crime") },
                { "Fantasy",Genre.Reconstruct(2, "Fantasy") }

            };
            referenceDataService.Setup(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(listOFGenres));
            tvSeriesRepository.Setup(t => t.AddListOfTvSeries(It.IsAny<List<TvSeries>>(), It.IsAny<CancellationToken>()));
            unitOfWork.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()));
            var result = await addListOfTvSeriesHandler.Handle(commands, CancellationToken.None);
            referenceDataService.Verify(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
            tvSeriesRepository.Verify(t => t.AddListOfTvSeries(It.IsAny<List<TvSeries>>(), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.HasCount(2, result);
            Assert.AreEqual("Breaking Bad", result[0].Title);
            Assert.AreEqual("Game of Thrones", result[1].Title);
        }
        [TestMethod]
        public async Task GetTvSeriesByCriteria()
        {
            var TvSeriesList = new List<TvSeries>
            {
                TvSeries.Reconstruct(1, "Breaking Bad", "A high school chemistry teacher turned methamphetamine producer.", new Language("English"), new ReleaseDate(new DateTime(2008, 1, 20)), 1, 5, 62, "AMC", EStatus.Completed, new MediaStats(6,1)),
                TvSeries.Reconstruct(2, "Game of Thrones", "Nine noble families fight for control over the lands of Westeros.", new Language("English"), new ReleaseDate(new DateTime(2011, 4, 17)), 2, 8, 73, "HBO", EStatus.Completed, new MediaStats(6,1))
            };
            var filteredList = TvSeriesList.Where(t => t.GenreId == 1).OrderByDescending(t => t.Title).ToList();
            sortAndFilterService.Setup(s => s.Handler(It.IsAny<GetTvSeriesByCriteriaQuery>())).Returns(filteredList.BuildMock());
            tvSeriesRepository.Setup(u => u.ToListAsync(It.IsAny<IQueryable<TvSeries>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(filteredList));
            var genresDictionary = new Dictionary<int, Genre>
            {
                { 1, Genre.Reconstruct(1, "Crime" )  },
                { 2, Genre.Reconstruct(2, "Fantasy" ) }
            };
            genreRepository.Setup(u => u.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genresDictionary));
            var query = new GetTvSeriesByCriteriaQuery
            {
                genreName = "Crime",
                SortByField = "Title",
                IsDescending = true
            };
            var result = await criteriaHandler.Handle(query, CancellationToken.None);
            sortAndFilterService.Verify(s => s.Handler(It.IsAny<GetTvSeriesByCriteriaQuery>()), Times.Once);
            tvSeriesRepository.Verify(u => u.ToListAsync(It.IsAny<IQueryable<TvSeries>>(), It.IsAny<CancellationToken>()), Times.Once);
            genreRepository.Verify(u => u.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual("Breaking Bad", result[0].Title);
            Assert.HasCount(1, result);
        }
    }
}
