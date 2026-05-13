using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Games.DeleteById;
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
    public class DeleteByIdHandlerTests
    {
        private readonly Guid gameId = Guid.NewGuid();
        private AppDbContext context;
        private DeleteByIdHandler handler;
        private IMediaRepository<Game> repository;
        [TestInitialize]
        public async Task TestInitialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            repository = new MediaRepository<Game>(context);
            handler = new DeleteByIdHandler(repository);
            await SeedData();
        }
        private async Task SeedData()
        {
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            context.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), genreId, "developer", new List<EPlatform>() { EPlatform.PC}, gameId);
            context.Medias.Add(game);
            await context.SaveChangesAsync();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            context.Dispose();
        }
        [TestMethod]
        public async Task Handle_DeleteGameById_ShouldDeleteGameFromDb()
        {
            var result = await handler.Handle(new DeleteByIdCommand(gameId), CancellationToken.None);
            await context.SaveChangesAsync();

            var gameInDb = await context.Medias.FindAsync(gameId);
            Assert.IsNull(gameInDb, "Gra powinna zostać usunięta z bazy danych.");
        }
        [TestMethod]
        public async Task Handle_DeleteById_ShouldThrowNotFoundException()
        {
            var nonExistentGameId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await handler.Handle(new DeleteByIdCommand(nonExistentGameId), CancellationToken.None);
            });
        }
    }
}
