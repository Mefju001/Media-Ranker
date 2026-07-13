using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Games.GetByCriteria;
using Domain.Aggregate;
using Domain.Enums;
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
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace Tests.Service.GameService
{
    [TestClass ]
    public class GetByHandlerTests
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
            services.AddValidatorsFromAssembly(typeof(GetByCriteriaQuery).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(GetByCriteriaHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Media>, MediaRepository<Media>>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
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
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game B", "Description B", "English", new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, new GameDetails("Developer B", "Engine B"), 3, new List<EPlatform>() { EPlatform.PC }, EGameStatus.Announced, true);
            appDbContext.Medias.Add(game);
            var game2 = Game.Create("Game A", "Description A", "English", new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, new GameDetails("Developer A", "Engine A"), 3, new List<EPlatform>() { EPlatform.PlayStation5 }, EGameStatus.Announced, true);
            appDbContext.Medias.Add(game2);
            appDbContext.SaveChanges();
        }


        [TestMethod]
        public async Task GetGamesByCriteria_WhenFilterByTitle_ShouldReturnMatch() 
        {
            List<GameResponse> result;
            using(var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery("Game A", null, null, null, null, null, null, true);
                result = await mediator.Send(query, CancellationToken.None);
            }
            Assert.HasCount(1, result);
            Assert.AreEqual("Game A", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered() 
        {
            List<GameResponse> result;
            using(var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery(null, null, null, null, null, null, "Date", true);
                result = await mediator.Send(query, CancellationToken.None);
            }
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            List<GameResponse> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery(null, null, null, null, null, null, null, true);
                result = await mediator.Send(query, CancellationToken.None);
            }
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
    }
}
