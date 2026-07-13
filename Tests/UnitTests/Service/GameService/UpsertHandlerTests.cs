using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Games.GetByCriteria;
using Application.Features.Games.Upsert;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
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


namespace Tests.Service.GameService
{
    [TestClass]
    public class UpsertHandlerTests
    {
        private Guid GameId;
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public async Task Setup()
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
            services.AddValidatorsFromAssembly(typeof(UpsertCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(UpsertHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Game>, MediaRepository<Game>>();
            services.AddScoped<IGenreManager, GenreManager>();
            services.AddScoped<IGenreRepository, GenreRepository>();
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
            var genre = Genre.Create("Action", Guid.NewGuid());
            context.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            context.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", "English", new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, new GameDetails("Developer A","Engine"), 3, new List<EPlatform>() { EPlatform.PlayStation5 }, EGameStatus.Announced, true);
            context.Medias.Add(game);
            GameId = game.Id;
            context.SaveChanges();
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNull_ShouldCreateNewGame()
        {
            var command = new UpsertCommand(
                null,
                "New Game",
                "Description",
                new GenreRequest("Action"),
                DateTime.UtcNow,
                "EN",
                "Announced",
                "Dev",
                "Engine",
                3,
                new List<String> { "PC" },
                true
                );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Game");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Game", gameInDb.Title);

        }
        [TestMethod]
        public async Task Handle_WhenIdIsNotNull_ShouldUpdateExistingGame()
        {
            var command = new UpsertCommand(
                            GameId,
                            "New Game",
                            "Description",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Announced",
                            "Dev",
                            "Engine",
                            3,
                            new List<String> { "PC" },
                            true
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Game");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Game", gameInDb.Title);
            Assert.AreEqual("Description", gameInDb.Description);
        }
        [TestMethod]
        public async Task Handle_WhenGenreDoesNotExist_ShouldCreateNewGenre()
        {
            var command = new UpsertCommand(
                null,
                "New Game",
                "Description",
                new GenreRequest("New Genre"),
                DateTime.UtcNow,
                "EN",
                "Announced",
                "Dev",
                "Engine",
                3,
                new List<String> { "PC" },
                true
                );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Name == "New Genre");
            Assert.IsNotNull(genreInDb);
            Assert.AreEqual("New Genre", genreInDb.Name);
        }
        [TestMethod]
        public async Task Handle_GenreRequestIsEmpty_ShouldThrowArgumentException()
        {
            var command = new UpsertCommand(
                            null,
                            "New Game",
                            "Description",
                            new GenreRequest(string.Empty),
                            DateTime.UtcNow,
                            "EN",
                            "Announced",
                            "Dev",
                            "Engine",
                            3,
                            new List<String> { "PC" },
                            true
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_WhenGameDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new UpsertCommand(
                            Guid.NewGuid(),
                            "New Game",
                            "Description",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Announced",
                            "Dev",
                            "Engine",
                            3,
                            new List<String> { "PC" },
                            true
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsAsync<NotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ChangeGenreToExisting_ShouldUpdateGameGenre()
        {
            var command = new UpsertCommand(
                            GameId,
                            "Game A",
                            "Description A",
                            new GenreRequest("Action"),
                            DateTime.UtcNow,
                            "EN",
                            "Announced",
                            "Dev",
                            "Engine",
                            3,
                            new List<String>() { "PlayStation5" },
                            true
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Id == GameId);
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Id == gameInDb.GenreId);
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("Action", genreInDb.Name);
        }
    }
}
