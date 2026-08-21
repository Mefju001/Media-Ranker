using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.GetTheLastestTitle;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Repository;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Tests.Service.ReviewService
{
    [TestClass]
    public class GetTheLastestTitleHandlerTests
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
            services.AddValidatorsFromAssembly(typeof(GetTheLastestTitleQuery).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(GetTheLastestTitleHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userModel = new UserModel(Guid.NewGuid(), "username", "password", "email@example.com");
            appDbContext.Users.Add(userModel);
            var user = UserDetails.Create(userModel.Id, new Fullname("testuser", "testuser"), "testuser", Email.Create("testuser@example.com"));
            appDbContext.UsersDetails.Add(user);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", "English", new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, new GameDetails("Developer A", "Engine A"), 3, new List<EPlatform>() { EPlatform.PlayStation5 }, EGameStatus.Announced, true);
            game.AddReview(userModel.Id, new Rating(4), "Good game!", "testuser2");
            appDbContext.Medias.Add(game);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task GetTheLastestTitle_Should_Return_List_Of_The_Lastest_Titles()
        {
            await SeedData();
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new GetTheLastestTitleQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
        }
        [TestMethod]
        public async Task GetTheLastestTitle_Should_Return_Empty_List_Of_The_Lastest_Titles()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new GetTheLastestTitleQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
    }
}
