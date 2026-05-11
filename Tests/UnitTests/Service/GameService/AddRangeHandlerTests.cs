using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Games.Command;
using Domain.Aggregate;
using Domain.Enums;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Application.Features.Genres.GenreManager;
using Application.Features.Games.AddRange;
using Application.Features.Genres.Common;

namespace Tests.Service.GameService
{
    [TestClass]
    public class AddRangeHandlerTests
    {
        private AppDbContext context;
        private Mock<IMediator> mockMediator;
        private AddRangeHandler handler;
        private IGenreManager genreHelperService;
        private IMediaRepository<Game> gameRepository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            genreHelperService = new GenreManager(new GenreRepository(context));
            gameRepository = new MediaRepository<Game>(context);
            mockMediator = new Mock<IMediator>();
            handler = new AddRangeHandler(mockMediator.Object, genreHelperService, gameRepository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        [TestMethod]
        public async Task Handle_AddTwoGames_ShouldCreateTwoGames()
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
            var command = new AddRangeCommand(listOfGames);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            mockMediator.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
            var gamesInDb = await context.Medias.ToListAsync();
            Assert.IsNotNull(gamesInDb);
            Assert.IsTrue(gamesInDb.Any(g => g.Title == "Game 1"));
            Assert.IsTrue(gamesInDb.Any(g => g.Title == "Game 2"));
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            var command = new AddRangeCommand(new List<GameRequest>());
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_AddGameWithExistingGenre_ShouldCreateGameWithExistingGenre()
        {
            var existingGenre = Genre.Create("Existing Genre", Guid.NewGuid());
            context.Genres.Add(existingGenre);
            await context.SaveChangesAsync();
            var listOfGames = new List<GameRequest>
            {
                new GameRequest
                ("Game 1",
                "Description 1",
                new GenreRequest("Existing Genre"),
                DateTime.UtcNow,
                "English",
                "Developer 1",
                new List<EPlatform> { EPlatform.PC }
                )
            };
            var command = new AddRangeCommand(listOfGames);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "Game 1");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual(existingGenre.Id, gameInDb.GenreId);
        }
        [TestMethod]
        public async Task Handle_OneGameInvalid_ShouldThrowExceptionAndAddNothing()
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
                    string.Empty, // Invalid title
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    DateTime.UtcNow,
                    "English",
                    "Developer 2",
                    new List<EPlatform> { EPlatform.XboxOne }
                )
            };
            var command = new AddRangeCommand(listOfGames);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(command, CancellationToken.None));
            await context.SaveChangesAsync();
            var count = await context.Medias.CountAsync();
            Assert.AreEqual(0, count);
            
        }
        [TestMethod]
        public async Task Handle_MultipleGamesWithSameNewGenre_ShouldCreateOnlyOneGenre()
        {
            var listOfGames = new List<GameRequest>
            {
                new GameRequest("Game 1", "Desc", new GenreRequest("New Genre"), DateTime.UtcNow, "EN", "Dev", new List<EPlatform>{EPlatform.PC}),
                new GameRequest("Game 2", "Desc", new GenreRequest("New Genre"), DateTime.UtcNow, "EN", "Dev", new List<EPlatform>{EPlatform.PC})
            };
            var command = new AddRangeCommand(listOfGames);

            await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();

            var genresInDb = await context.Genres.Where(g => g.Name.Value == "New Genre").ToListAsync();
            Assert.AreEqual(1, genresInDb.Count, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
        }
    }
}
