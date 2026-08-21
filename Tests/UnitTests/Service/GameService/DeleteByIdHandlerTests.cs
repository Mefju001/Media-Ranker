using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Medias.Games.DeleteById;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Service.GameService
{
    [TestClass]
    public class DeleteByIdHandlerTests
    {
        private readonly Guid gameId = Guid.NewGuid();
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        [TestInitialize]
        public async Task TestInitialize()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());
            services.AddValidatorsFromAssembly(typeof(DeleteByIdCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DeleteByIdHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Game>, MediaRepository<Game>>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            await SeedData();
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            context.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, new GameDetails("Developer A", "Engine A"), 3, new List<EPlatform>() { EPlatform.PC }, EGameStatus.Announced, true, gameId);
            context.Medias.Add(game);
            await context.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_DeleteGameById_ShouldDeleteGameFromDb()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var result = await mediator.Send(new DeleteByIdCommand(gameId), CancellationToken.None);
            }
            using(var scope2 = _serviceProvider.CreateScope())
            {
                var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
                var gameInDb = await context.Medias.FindAsync(gameId);
                Assert.IsNull(gameInDb, "Gra powinna zostać usunięta z bazy danych.");
            }
        }
        [TestMethod]
        public async Task Handle_DeleteById_ShouldThrowNotFoundException()
        {
            var nonExistentGameId = Guid.NewGuid();
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                {
                    await mediator.Send(new DeleteByIdCommand(nonExistentGameId), CancellationToken.None);
                });
            }
        }
    }
}
