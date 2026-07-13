using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Reviews.Upsert;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
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
    public class UpsertHandlerTests
    {
        private string username;
        private Guid mediaId;
        private Guid userId;
        private Guid reviewId;
        private Guid secondUserId;
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
            services.AddScoped<IMediaRepository<Media>, MediaRepository<Media>>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            await SeedData();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.ChangeTracker.Clear();
            }
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userModel = new UserModel(Guid.NewGuid(), "username", "password", "email");
            userId = userModel.Id;
            var user = UserDetails.Create(userId, new Fullname("testuser", "testuser"), "testuser", Email.Create("testuser@example.com"));
            username = user.Username;
            var SecondUserModel = new UserModel(Guid.NewGuid(), "testuser2", "password", "email");
            secondUserId = SecondUserModel.Id;
            var secondUser = UserDetails.Create(secondUserId, new Fullname("testuser2", "testuser2"), "testuser2", Email.Create("testuser@example.com"));
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", "English", new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, new GameDetails("Developer A", "Engine A"), 3, new List<EPlatform>() { EPlatform.PlayStation5 }, EGameStatus.Announced, true);
            mediaId = game.Id;
            game.AddReview(secondUserId, new Rating(4), "Good game!", "testuser2");
            reviewId = game.Reviews.First(r => r.UserId == secondUserId).Id;
            appDbContext.Medias.Add(game);
            appDbContext.UsersDetails.Add(user);
            appDbContext.Users.Add(userModel);
            appDbContext.UsersDetails.Add(secondUser);
            appDbContext.Users.Add(SecondUserModel);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task AddReview_ShouldAddReview()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new UpsertCommand(null, mediaId, userId, 5, "Great game!");
            var result = await mediator.Send(command, CancellationToken.None);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual(username, result.username);
        }
        [TestMethod]
        public async Task UpdateReview_ShouldUpdateExistingReview()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new UpsertCommand(reviewId, mediaId, secondUserId, 3, "Updated comment");
            var result = await mediator.Send(command, CancellationToken.None);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual("testuser2", result.username);
        }
        [TestMethod]
        public async Task UpsertReview_MissingUserId_ShouldThrowArgumentException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new UpsertCommand(null, mediaId, null, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task UpsertReview_MissingMediaId_ShouldThrowArgumentException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new UpsertCommand(null, null, userId, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task UpsertReview_NonExistentMedia_ShouldThrowNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new UpsertCommand(null, Guid.NewGuid(), userId, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
        }
    }
}
