using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.GamesServices.AddListOfGames;
using Application.Features.GamesServices.GameUpsert;
using Application.Features.GamesServices.GetAll;
using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Domain.Entity;
using Domain.Enums;
using Domain.Value_Object;
using MediatR;
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
        private Mock<IMediator> mockMediator;
        private Mock<IReferenceDataService> referenceDataService;
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IGameSortAndFilterService> gameSortAndFilterService;
        /*[TestInitialize]
         public void Setup()
         {
             mockUnitOfWork = new Mock<IUnitOfWork>();
             referenceDataService = new Mock<IReferenceDataService>();
             mockMediator = new Mock<IMediator>();
             gameSortAndFilterService = new Mock<IGameSortAndFilterService>();
             handlerUpsert = new GameUpsertHandler(mockUnitOfWork.Object, referenceDataService.Object, mockMediator.Object);
             handlerGetAll = new GetAllHandler(mockUnitOfWork.Object);
             handlerAddListOfGames = new AddListOfGamesHandler(referenceDataService.Object, mockUnitOfWork.Object);
             getGamesByCriteriaHandler = new GetGamesByCriteriaHandler(mockUnitOfWork.Object,gameSortAndFilterService.Object);
         }
         [TestMethod]
         public async Task GamesGetAll()
         {
             List<Game> games = new List<Game>
             {
                 Game.Reconstruct(1, "Game 1", "Description 1", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Developer 1", EPlatform.PC, new MediaStats(6,2)),
                 Game.Reconstruct(2, "Game 2", "Description 2", new Language("English"), new ReleaseDate(DateTime.UtcNow), 2, "Developer 2", EPlatform.Xbox,new MediaStats(6,3))
             };
             Dictionary<int, Genre> genres = new Dictionary<int, Genre>
             {
                 { 1,Genre.Reconstruct(1, "Genre 1") },
                 { 2,Genre.Reconstruct(2, "Genre 2") }
             };
             mockUnitOfWork.Setup(u => u.GameRepository.GetAllAsync()).Returns(Task.FromResult(games));
             mockUnitOfWork.Setup(u => u.GenreRepository.GetGenresDictionary()).Returns(Task.FromResult(genres));
             var result = await handlerGetAll.Handle(new GetAllQuery(), CancellationToken.None);
             Assert.HasCount(2, result);
             Assert.AreEqual("Game 1", result[0].Title);
             Assert.AreEqual("Game 2", result[1].Title);
             mockUnitOfWork.Verify(u => u.GameRepository.GetAllAsync(), Times.Once);
             mockUnitOfWork.Verify(u => u.GenreRepository.GetGenresDictionary(), Times.Once);
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
             var gameDomain = Game.Reconstruct(1, "New Game", "New Description", new Language("Polski"), null, 1, "CD Projekt Red", EPlatform.PC, new MediaStats(6, 2));
             referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).Returns(Task.FromResult(genreDomain));
             mockUnitOfWork.Setup(u => u.GameRepository.AddGameAsync(It.IsAny<Game>())).Returns(Task.FromResult(gameDomain));
             mockUnitOfWork.Setup(u => u.GenreRepository.Get(It.IsAny<int>())).Returns(Task.FromResult(genreDomain));
             var result = await handlerUpsert.Handle(command, CancellationToken.None);
             Assert.IsNotNull(result);
             Assert.AreEqual("New Game", result.Title);
             Assert.AreEqual("New Description", result.Description);
             Assert.AreEqual("Polski", result.Language);
             Assert.AreEqual("CD Projekt Red", result.Developer);
             Assert.AreEqual(EPlatform.PC, result.Platform);
             Assert.AreEqual("new name", result.Genre.Name);
             referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GameRepository.AddGameAsync(It.IsAny<Game>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GenreRepository.Get(It.IsAny<int>()), Times.Once);
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
             var gameDomain = Game.Reconstruct(1, "old game", "New Description", new Language("Polski"), null, 1, "CD Projekt Red", EPlatform.PC, new MediaStats(6, 2));
             referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).Returns(Task.FromResult(genreDomain));
             mockUnitOfWork.Setup(u => u.GameRepository.GetGameDomainAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(gameDomain));
             mockUnitOfWork.Setup(u => u.GenreRepository.Get(It.IsAny<int>())).Returns(Task.FromResult(genreDomain));
             var result = await handlerUpsert.Handle(command, CancellationToken.None);
             Assert.IsNotNull(result);
             Assert.AreEqual("New Game", result.Title);
             referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GameRepository.GetGameDomainAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GenreRepository.Get(It.IsAny<int>()), Times.Once);
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
             var genreDictionary = new Dictionary<String, GenreDomain>
             {
                 { "Genre 1", GenreDomain.Reconstruct(1, "Genre 1") },
                 { "Genre 2", GenreDomain.Reconstruct(2, "Genre 2") }
             };
             referenceDataService.Setup(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>())).Returns(Task.FromResult(genreDictionary));
             mockUnitOfWork.Setup(u => u.GameRepository.AddListOfGames(It.IsAny<List<GameDomain>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
             var command = new AddListOfGamesCommand(listOfGames);
             var result = await handlerAddListOfGames.Handle(command, CancellationToken.None);
             Assert.IsNotNull(result);
             Assert.HasCount(2, result);
             referenceDataService.Verify(r => r.EnsureGenresExistAsync(It.IsAny<List<string>>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GameRepository.AddListOfGames(It.IsAny<List<GameDomain>>(), It.IsAny<CancellationToken>()), Times.Once);
         }
         [TestMethod]
         public async Task GetGamesByCriteria()
         {
             /*List<GameDomain> games = new List<GameDomain>
             {
                 GameDomain.Reconstruct(1, "Game 1", "Description 1", "English", DateTime.Now, 1, "Developer 1", EPlatform.PC),
                 GameDomain.Reconstruct(2, "Game 2", "Description 2", "English", DateTime.Now, 2, "Developer 2", EPlatform.Xbox)
             };
             var genreDictionary = new Dictionary<int, GenreDomain>
             {
                 { 1, GenreDomain.Reconstruct(1, "Genre 1") },
                 { 2, GenreDomain.Reconstruct(2, "Genre 2") }
             };
             gameSortAndFilterService.Setup(s => s.ApplyFiltersAsync(It.IsAny<GetGamesByCriteriaQuery>())).Returns(Task.FromResult(games.Where(g=>g.GenreId == 1 ).ToList().BuildMock()));
             gameSortAndFilterService.Setup(m => m.ApplySorting(It.IsAny<IQueryable<GameDomain>>(), It.IsAny<GetGamesByCriteriaQuery>())).Returns(Task.FromResult(games.Where(m => m.GenreId == 1).OrderByDescending(m => m.Title).ToList().BuildMock()));
             mockUnitOfWork.Setup(u => u.GenreRepository.GetGenresDictionary()).Returns(Task.FromResult(genreDictionary));
             var query = new GetGamesByCriteriaQuery
             {
                 genreName = "Genre 1",
                 sortByField = "Title",
                 IsDescending = true
             };
             var result = await getGamesByCriteriaHandler.Handle(query, CancellationToken.None);
             gameSortAndFilterService.Verify(s => s.ApplyFiltersAsync(It.IsAny<GetGamesByCriteriaQuery>()), Times.Once);
             gameSortAndFilterService.Verify(m => m.ApplySorting(It.IsAny<IQueryable<GameDomain>>(), It.IsAny<GetGamesByCriteriaQuery>()), Times.Once);
             mockUnitOfWork.Verify(u => u.GenreRepository.GetGenresDictionary(), Times.Once);
             Assert.IsNotNull(result);
             Assert.HasCount(1, result);
             Assert.AreEqual("Game 1", result[0].Title);

         }*/
    }
}
