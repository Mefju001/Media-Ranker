using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.Liked.Add;
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


namespace Tests.Service.LikedService
{
    [TestClass]
    public class AddHandlerTests
    {
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        private Guid userId;
        private Guid mediaId;
        [TestInitialize]
        public async Task setup()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
            services.AddScoped<IAppDbContext>(provider=>
                provider.GetRequiredService<AppDbContext>());
            services.AddValidatorsFromAssembly(typeof(AddCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(AddHandler).Assembly);
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
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = new UserModel(Guid.NewGuid(), "username", "password", "email");
            userId = user.Id;
            var userDetails = UserDetails.Create(userId, new Fullname("Name", "Surname"), "username", Email.Create("email@example.com"));

            db.Users.Add(user);
            db.UsersDetails.Add(userDetails);

            var genre = Genre.Create("Name");
            var game = Game.Create("Title", "Desc", "Eng", new ReleaseDate(DateTime.UtcNow.AddDays(-1)), genre.Id, new GameDetails("Dev","Engine"), 3, new List<EPlatform> { EPlatform.PC }, EGameStatus.Announced, true);
            mediaId = game.Id;

            db.Genres.Add(genre);
            db.Medias.Add(game);

            await db.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_AddLiked_ShouldAddLikedMedia()
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddCommand(userId, mediaId);
                result = await mediator.Send(command);
            }

            using (var assertScope = _serviceProvider.CreateScope())
            {
                var db = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var likedMedia = await db.UserInteractions
                    .FirstOrDefaultAsync(lm => lm.UserId == userId && lm.MediaId == mediaId);
                Assert.IsTrue(result);
                Assert.IsNotNull(likedMedia);
                Assert.AreEqual(userId, likedMedia.UserId);
                Assert.AreEqual(mediaId, likedMedia.MediaId);
            }

            
        }

        [TestMethod]
        public async Task Handle_AddLikedWhereUserIdIsNull_ShouldThrowNotFoundException()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var command = new AddCommand(Guid.NewGuid(), mediaId);

                await Assert.ThrowsExactlyAsync<NotFoundException>(() => mediator.Send(command));
            }
        }
    }
}
