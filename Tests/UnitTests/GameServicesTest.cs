using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
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
        private GameUpsertHandler handlerUpsert;
        private AddListOfGamesHandler handlerAddListOfGames;
        private Mock<IMediaRepository<Game>> mockGameRepository;
        private Mock<IGenreHelperService> mockGenreHelperService;
        private Mock<IMediator> mockMediator;
        private Mock<IGameSortAndFilterService> gameSortAndFilterService;
        private void setupMocks()
        {
            mockMediator = new Mock<IMediator>();
            mockGenreHelperService = new Mock<IGenreHelperService>();
            gameSortAndFilterService = new Mock<IGameSortAndFilterService>();
            mockGameRepository = new Mock<IMediaRepository<Game>>();
        }
        [TestInitialize]
        public void Setup()
        {
            setupMocks();
            handlerUpsert = new GameUpsertHandler(mockGenreHelperService.Object, mockMediator.Object, mockGameRepository.Object);
            handlerAddListOfGames = new AddListOfGamesHandler(mockMediator.Object, mockGenreHelperService.Object, mockGameRepository.Object);
            getGamesByCriteriaHandler = new GetGamesByCriteriaHandler(gameSortAndFilterService.Object);
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
                new List<EPlatform> { EPlatform.PC });
            var genreDomain = Genre.Create("new name",Guid.NewGuid());
            var gameDomain = Game.Create("New Game", "New Description", new Language("Polski"), new ReleaseDate(DateTime.UtcNow), genreDomain.Id, "CD Projekt Red", new List<EPlatform> { EPlatform.PC });
            mockGenreHelperService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDomain));
            mockGameRepository.Setup(u => u.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(gameDomain));
            var result = await handlerUpsert.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual("New Game", result.Title);
            Assert.AreEqual("New Description", result.Description);
            Assert.AreEqual("Polski", result.Language);
            Assert.AreEqual("CD Projekt Red", result.Developer);
            Assert.AreEqual(EPlatform.PC, result.Platforms.FirstOrDefault(x => x.CompareTo(EPlatform.PC) == 0));
            Assert.AreEqual("new name", result.Genre.Name);
            mockGenreHelperService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task GamesUpsert_UpdateGame()
        {
            var command = new UpsertGameCommand(
            Guid.NewGuid(),
            "New Game",
            "New Description",
            new GenreRequest("new name"),
            DateTime.UtcNow,
            "Polski",
            "CD Projekt Red",
            new List<EPlatform> { EPlatform.PC });
            var genreDomain = Genre.Create("new name", Guid.NewGuid());
            var gameDomain = Game.Create("old game", "New Description", new Language("Polski"), new ReleaseDate(DateTime.UtcNow), genreDomain.Id, "CD Projekt Red", new List<EPlatform> { EPlatform.PC }, command.id);
            mockGenreHelperService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDomain));
            mockGameRepository.Setup(u => u.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(gameDomain));
            var result = await handlerUpsert.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual("New Game", result.Title);
            mockGenreHelperService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
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
                new List<EPlatform> { EPlatform.PC }
                ),
                new GameRequest
                (
                    "Game 2",
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    DateTime.UtcNow,
                    "English",
                    "Developer 2",
                    new List<EPlatform> { EPlatform.XboxOne }
                )
            };
            var genreDictionary = new Dictionary<String, Genre>
            {
                { "Genre 1", Genre.Create("Genre 1", Guid.NewGuid()) },
                { "Genre 2", Genre.Create("Genre 2", Guid.NewGuid()) }
            };
            mockGenreHelperService.Setup(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(genreDictionary));
            mockGameRepository.Setup(u => u.AddRangeAsync(It.IsAny<List<Game>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var command = new AddListOfGamesCommand(listOfGames);
            var result = await handlerAddListOfGames.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            mockGenreHelperService.Verify(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockGameRepository.Verify(u => u.AddRangeAsync(It.IsAny<List<Game>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task GetGamesByCriteria()
        {
            var genreId1 = Guid.NewGuid();
            var genreId2 = Guid.NewGuid();
            var genreDictionary = new Dictionary<Guid, Genre>
            {
                { genreId1, Genre.Create("Genre 1", genreId1) },
                { genreId2, Genre.Create("Genre 2", genreId2) }
            };
            List<GameResponse> games = new List<GameResponse>
            {
                new GameResponse(Guid.NewGuid(), "Game 1", "Description 1", new GenreResponse(genreId1, "Genre 1"), DateTime.UtcNow, "English", new List<ReviewResponse>(), new MediaStatsResponse(0.0,0,DateTime.UtcNow), "Developer 1", new List<EPlatform> { EPlatform.PC }),
            };
            gameSortAndFilterService.Setup(s => s.GetGamesByCriteriaAsync(It.IsAny<GetGamesByCriteriaQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(games));
            var query = new GetGamesByCriteriaQuery 
            {
                genreName = "Genre 1",
                sortByField = "Title",
                IsDescending = true
            };
            var result = await getGamesByCriteriaHandler.Handle(query, CancellationToken.None);
            gameSortAndFilterService.Verify(s => s.GetGamesByCriteriaAsync(It.Is<GetGamesByCriteriaQuery>(q =>
                    q.genreName == "Genre 1" &&
                    q.sortByField == "Title" &&
                    q.IsDescending == true),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            Assert.AreEqual("Game 1", result[0].Title);
        }
    }
}
