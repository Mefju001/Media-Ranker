using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace Tests.Service.GameService
{
    [TestClass]
    public class GameUpsert
    {
        private Guid GameId;
        private AppDbContext context;
        private IMediaRepository<Game> repository;
        private Mock<IMediator> mediatorMock;
        private GenreHelperService genreHelperMock;
        private GameUpsertHandler handler;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            genreHelperMock = new GenreHelperService(new GenreRepository(context));
            repository = new MediaRepository<Game>(context);
            mediatorMock = new Mock<IMediator>();
            handler = new GameUpsertHandler(genreHelperMock, mediatorMock.Object, repository);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        private async Task SeedData()
        {
            var genre = Genre.Create("Action", Guid.NewGuid());
            context.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            context.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            context.Medias.Add(game);
            GameId = game.Id;
            context.SaveChanges();
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNull_ShouldCreateNewGame()
        {
            var command = new UpsertGameCommand(
                null,
                "New Game",
                "Description",
                new GenreRequest("Action"),
                DateTime.UtcNow,
                "EN",
                "Dev",
                new List<EPlatform> { EPlatform.PC }
                );


            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Game");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Game", gameInDb.Title);

            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNotNull_ShouldUpdateExistingGame()
        {
            var command = new UpsertGameCommand(
                            GameId,
                            "New Game",
                            "Description",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Dev",
                            new List<EPlatform> { EPlatform.PC }
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Game");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Game", gameInDb.Title);
            Assert.AreEqual("Description", gameInDb.Description);
            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("zaktualizowana")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Handle_WhenGenreDoesNotExist_ShouldCreateNewGenre()
        {
            var command = new UpsertGameCommand(
                null,
                "New Game",
                "Description",
                new GenreRequest("New Genre"),
                DateTime.UtcNow,
                "EN",
                "Dev",
                new List<EPlatform> { EPlatform.PC }
                );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Name.Value == "New Genre");
            Assert.IsNotNull(genreInDb);
            Assert.AreEqual("New Genre", genreInDb.Name.Value);
        }
        [TestMethod]
        public async Task Handle_GenreRequestIsEmpty_ShouldThrowArgumentException()
        {
            var command = new UpsertGameCommand(
                            null,
                            "New Game",
                            "Description",
                            new GenreRequest(string.Empty),
                            DateTime.UtcNow,
                            "EN",
                            "Dev",
                            new List<EPlatform> { EPlatform.PC }
                            );
            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_WhenGameDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new UpsertGameCommand(
                            Guid.NewGuid(),
                            "New Game",
                            "Description",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Dev",
                            new List<EPlatform> { EPlatform.PC }
                            );
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ChangeGenreToExisting_ShouldUpdateGameGenre()
        {
            var command = new UpsertGameCommand(
                            GameId,
                            "Game A",
                            "Description A",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Developer A",
                            new List<EPlatform>() { EPlatform.PlayStation5 }
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Id == GameId);
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Id == gameInDb.GenreId);
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("Action", genreInDb.Name.Value);
        }
    }
}
