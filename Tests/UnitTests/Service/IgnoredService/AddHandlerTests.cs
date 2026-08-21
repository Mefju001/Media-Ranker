using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.Ignored.Add;
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

namespace Tests.Service.IgnoredService
{
    [TestClass]
    public class AddHandlerTests
    {
        private Guid GameId, UserId;
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
            services.AddValidatorsFromAssembly(typeof(AddHandler).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(AddCommand).Assembly);
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
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = new UserModel(Guid.NewGuid(), "username", "password", "email");
            UserId = user.Id;
            var userDetails = UserDetails.Create(UserId, new Fullname("Name", "Surname"), "username", Email.Create("email@example.com"));

            db.Users.Add(user);
            db.UsersDetails.Add(userDetails);

            var genre = Genre.Create("Name");
            var game = Game.Create("Title", "Desc", "Eng", new ReleaseDate(DateTime.UtcNow.AddDays(-1)), genre.Id, new GameDetails("Dev","Engine"), 3, new List<EPlatform> { EPlatform.PC }, EGameStatus.Announced, true);
            GameId = game.Id;

            db.Genres.Add(genre);
            db.Medias.Add(game);

            await db.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_AddIgnored_ShouldAddIgnoredMedia()
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddCommand(GameId, UserId);
                result = await mediator.Send(command);
            }

            using (var assertScope = _serviceProvider.CreateScope())
            {
                var db = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var likedMedia = await db.UserInteractions
                    .FirstOrDefaultAsync(lm => lm.UserId == UserId && lm.MediaId == GameId);
                Assert.IsTrue(result);
                Assert.IsNotNull(likedMedia);
                Assert.AreEqual(UserId, likedMedia.UserId);
                Assert.AreEqual(GameId, likedMedia.MediaId);
            }


        }

        [TestMethod]
        public async Task Handle_AddIgnoredWhereUserIdIsNull_ShouldThrowNotFoundException()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var command = new AddCommand(Guid.NewGuid(), GameId);

                await Assert.ThrowsExactlyAsync<NotFoundException>(() => mediator.Send(command));
            }
        }
    }
}
