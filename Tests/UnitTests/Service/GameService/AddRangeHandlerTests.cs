using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Games.AddRange;
using Application.Features.Games.Command;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Repository;
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
    public class AddRangeHandlerTests
    {
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
            services.AddValidatorsFromAssembly(typeof(AddRangeCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(AddRangeHandler).Assembly);
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
                new List<String> { "PC" }
                ),
                new GameRequest
                (
                    "Game 2",
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    DateTime.UtcNow,
                    "English",
                    "Developer 2",
                    new List<String> { "PC" }
                )
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfGames);
                var result = await mediator.Send(command, CancellationToken.None);
            }
            using (var scope2 = _serviceProvider.CreateScope())
            {
                var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
                var gamesInDb = await context.Medias.ToListAsync();
                Assert.IsNotNull(gamesInDb);
                Assert.IsTrue(gamesInDb.Any(g => g.Title == "Game 1"));
                Assert.IsTrue(gamesInDb.Any(g => g.Title == "Game 2"));
            }
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            List<GameResponse> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(new List<GameRequest>());
                result = await mediator.Send(command, CancellationToken.None);
            }
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_AddGameWithExistingGenre_ShouldCreateGameWithExistingGenre()
        {
            List<GameResponse> result;
            using(var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var existingGenre = Genre.Create("Existing Genre", Guid.NewGuid());
                context.Genres.Add(existingGenre);
                await context.SaveChangesAsync();
            }
            var listOfGames = new List<GameRequest>
            {
                new GameRequest
                ("Game 1",
                "Description 1",
                new GenreRequest("Existing Genre"),
                DateTime.UtcNow,
                "English",
                "Developer 1",
                new List<String> { "PC" }
                )
            };
            using(var scope2 = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfGames);
                result = await mediator.Send(command, CancellationToken.None);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                var context = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var existingGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name.Value == "Existing Genre");
                Assert.IsNotNull(result);
                Assert.HasCount(1, result);
                var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "Game 1");
                Assert.IsNotNull(gameInDb);
                Assert.AreEqual(existingGenre.Id, gameInDb.GenreId);
            }
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
                new List<String> { "PC" }
                ),
                new GameRequest
                (
                    string.Empty, // Invalid title
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    DateTime.UtcNow,
                    "English",
                    "Developer 2",
                    new List<String> { "XboxOne" }
                )
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfGames);
                await Assert.ThrowsAsync<ArgumentException>(async () =>
                    await mediator.Send(command, CancellationToken.None));
            }
            using (var scope2 = _serviceProvider.CreateScope())
            {
                var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
                var count = await context.Medias.CountAsync();
                Assert.AreEqual(0, count);
            }

            
        }
        [TestMethod]
        public async Task Handle_MultipleGamesWithSameNewGenre_ShouldCreateOnlyOneGenre()
        {
            var listOfGames = new List<GameRequest>
            {
                new GameRequest("Game 1", "Desc", new GenreRequest("New Genre"), DateTime.UtcNow, "EN", "Dev", new List<String>{"PC"}),
                new GameRequest("Game 2", "Desc", new GenreRequest("New Genre"), DateTime.UtcNow, "EN", "Dev", new List<String>{"PC"})
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfGames);
                await mediator.Send(command, CancellationToken.None);
            }
            using ( var scope2 = _serviceProvider.CreateScope())
            {
                var context = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
                var genresInDb = await context.Genres.Where(g => g.Name.Value == "New Genre").ToListAsync();
                Assert.HasCount(1, genresInDb, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
            }
        }
    }
}

