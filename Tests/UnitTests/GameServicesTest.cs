using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.GamesServices.AddListOfGames;
using Application.Features.GamesServices.GameUpsert;
using Application.Features.GamesServices.GetGamesByCriteria;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace UnitTests
{
    [TestClass]
    public sealed class GameServicesTest
    {
        private GetGamesByCriteriaHandler getGamesByCriteriaHandler;
        private GetAllHandler handlerGetAll;
        private GameUpsertHandler handlerUpsert;
        private AddListOfGamesHandler handlerAddListOfGames;
        private Mock<IGameRepository> mockGameRepository;
        private Mock<IGenreRepository> mockGenreRepository;
        private Mock<IMediator> mockMediator;
        private Mock<IReferenceDataService> referenceDataService;
        private Mock<> mock;
        private Mock<IGameSortAndFilterService> gameSortAndFilterService;
        private void setupMocks()
        {
            mock = new Mock<>();
            referenceDataService = new Mock<IReferenceDataService>();
            mockMediator = new Mock<IMediator>();
            gameSortAndFilterService = new Mock<IGameSortAndFilterService>();
            mockGameRepository = new Mock<IGameRepository>();
            mockGenreRepository = new Mock<IGenreRepository>();
        }
        [TestInitialize]
        public void Setup()
        {
            setupMocks();
            handlerUpsert = new GameUpsertHandler(mock.Object, referenceDataService.Object, mockMediator.Object, mockGameRepository.Object, Mock.Of<ILogger<GameUpsertHandler>>());
            handlerGetAll = new GetAllHandler(mockGameRepository.Object, mockGenreRepository.Object);
            handlerAddListOfGames = new AddListOfGamesHandler(mockMediator.Object, referenceDataService.Object, mock.Object, mockGameRepository.Object, Mock.Of<ILogger<AddListOfGamesHandler>>());
            getGamesByCriteriaHandler = new GetGamesByCriteriaHandler(gameSortAndFilterService.Object, mockGameRepository.Object, mockGenreRepository.Object);
        }
        [TestMethod]
        public async Task GamesGetAll()
        {
            List<Game> games = new List<Game>
            {
                Game.Create(1, "Game 1", "Description 1", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Developer 1", EPlatform.PC, new MediaStats(6,2)),
                Game.Reconstruct(2, "Game 2", "Description 2", new Language("English"), new ReleaseDate(DateTime.UtcNow), 2, "Developer 2", EPlatform.Xbox,new MediaStats(6,3))
            };
            Dictionary<int, Genre> genres = new Dictionary<int, Genre>
            {
                { 1,Genre.Reconstruct(1, "Genre 1") },
                { 2,Genre.Reconstruct(2, "Genre 2") }
            };
            mockGameRepository.Setup(u => u.GetAllAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(games));
            mockGenreRepository.Setup(u => u.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genres));
            var result = await handlerGetAll.Handle(new GetAllQuery(), CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Game 1", result[0].Title);
            Assert.AreEqual("Game 2", result[1].Title);
            mockGameRepository.Verify(u => u.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockGenreRepository.Verify(u => u.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task GamesUpsert_AddGames()
        {
            var command = new UpsertGameCommand(
                null,
                "New Game",
                "New Description",
                new GenreRequest("new name"),
                DateTime.UtcNow,
                "Polski",
                "CD Projekt Red",
                EPlatform.PC);
            var genreDomain = Genre.Reconstruct(1, "new name");
            var gameDomain = Game.Reconstruct(1, "New Game", "New Description", new Language("Polski"), new ReleaseDate(DateTime.UtcNow), 1, "CD Projekt Red", EPlatform.PC, new MediaStats(6, 2));
            referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDomain));
            mockGameRepository.Setup(u => u.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(gameDomain));
            mock.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()));
            var result = await handlerUpsert.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual("New Game", result.Title);
            Assert.AreEqual("New Description", result.Description);
            Assert.AreEqual("Polski", result.Language);
            Assert.AreEqual("CD Projekt Red", result.Developer);
            Assert.AreEqual(EPlatform.PC, result.Platform);
            Assert.AreEqual("new name", result.Genre.Name);
            referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.AddGameAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Once);
            mock.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task GamesUpsert_UpdateGame()
        {
            var command = new UpsertGameCommand(
            1,
            "New Game",
            "New Description",
            new GenreRequest("new name"),
            DateTime.UtcNow,
            "Polski",
            "CD Projekt Red",
            EPlatform.PC);
            var genreDomain = Genre.Reconstruct(1, "new name");
            var gameDomain = Game.Reconstruct(1, "old game", "New Description", new Language("Polski"), new ReleaseDate(DateTime.UtcNow), 1, "CD Projekt Red", EPlatform.PC, new MediaStats(6, 2));
            referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDomain));
            mockGameRepository.Setup(u => u.GetGameDomainAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(gameDomain));
            mock.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()));
            var result = await handlerUpsert.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual("New Game", result.Title);
            referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.GetGameDomainAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            mock.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task AddListOfGames()
        {
            var listOfGames = new List<GameRequest>
            {
                new GameRequest
                ("Game 1",
                "Description 1",
                new GenreRequest("Genre 1"),
                DateTime.UtcNow,
                "English",
                "Developer 1",
                EPlatform.PC
                ),
                new GameRequest
                (
                    "Game 2",
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    DateTime.UtcNow,
                    "English",
                    "Developer 2",
                    EPlatform.Xbox
                )
            };
            var genreDictionary = new Dictionary<String, Genre>
            {
                { "Genre 1", Genre.Reconstruct(1, "Genre 1") },
                { "Genre 2", Genre.Reconstruct(2, "Genre 2") }
            };
            referenceDataService.Setup(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDictionary));
            mockGameRepository.Setup(u => u.AddListOfGames(It.IsAny<List<Game>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var command = new AddListOfGamesCommand(listOfGames);
            var result = await handlerAddListOfGames.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            referenceDataService.Verify(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.AddListOfGames(It.IsAny<List<Game>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task GetGamesByCriteria()
        {
            List<Game> games = new List<Game>
            {
                Game.Reconstruct(1, "Game 1", "Description 1", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Developer 1", EPlatform.PC, new MediaStats(6, 2)),
                Game.Reconstruct(2, "Game 2", "Description 2", new Language("English"), new ReleaseDate(DateTime.UtcNow), 2, "Developer 2", EPlatform.Xbox, new MediaStats(6, 2))
            };
            var genreDictionary = new Dictionary<int, Genre>
            {
                { 1, Genre.Reconstruct(1, "Genre 1") },
                { 2, Genre.Reconstruct(2, "Genre 2") }
            };
            gameSortAndFilterService.Setup(s => s.GetGamesByCriteriaAsync(It.IsAny<GetGamesByCriteriaQuery>())).Returns(games.Where(g => g.GenreId == 1).ToList().BuildMock());
            mockGameRepository.Setup(s => s.GetListFromQueryAsync(It.IsAny<IQueryable<Game>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(games.Where(g => g.GenreId == 1).ToList()));
            mockGenreRepository.Setup(u => u.GetGenresDictionary(It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDictionary));
            var query = new GetGamesByCriteriaQuery
            {
                genreName = "Genre 1",
                sortByField = "Title",
                IsDescending = true
            };
            var result = await getGamesByCriteriaHandler.Handle(query, CancellationToken.None);
            gameSortAndFilterService.Verify(s => s.GetGamesByCriteriaAsync(It.IsAny<GetGamesByCriteriaQuery>()), Times.Once);
            mockGameRepository.Verify(s => s.GetListFromQueryAsync(It.IsAny<IQueryable<Game>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGenreRepository.Verify(u => u.GetGenresDictionary(It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            Assert.AreEqual("Game 1", result[0].Title);

        }
    }
}
