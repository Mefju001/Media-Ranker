using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Liked.Add;
using Application.Features.Liked.GetAllForUser;
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


namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class GetAllForUserHandlerTests
    {
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        private Guid mediaId1;
        private Guid mediaId2;
        private Guid userId;
        [TestInitialize]
        public async Task Initialize()
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
            var userDetails = UserDetails.Create(userId, new Fullname("Name", "Surname"), new Username("username"), Email.Create("email@example.com"));

            db.Users.Add(user);
            db.UsersDetails.Add(userDetails);

            var genre = Genre.Create("Name");
            var gameA = Game.Create("Title A", "Desc", new Language("Eng"), new ReleaseDate(DateTime.UtcNow.AddDays(-1)), genre.Id, "Dev", new List<EPlatform> { EPlatform.PC });
            mediaId1 = gameA.Id;
            var gameB = Game.Create("Title B", "Desc", new Language("Eng"), new ReleaseDate(DateTime.UtcNow.AddDays(-1)), genre.Id, "Dev", new List<EPlatform> { EPlatform.PC });
            mediaId2 = gameB.Id;

            userDetails.SetInteraction(gameA.Id, ETypeInteractions.COMPLETED, ERatingVote.Liked);
            userDetails.SetInteraction(gameB.Id, ETypeInteractions.COMPLETED, ERatingVote.Liked);
            db.Genres.Add(genre);
            db.Medias.Add(gameA);
            db.Medias.Add(gameB);

            await db.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_GetAllLikedForUser_ShouldReturnListOfMedias()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new GetAllForUserQuery(userId);

                var result = await mediator.Send(command);
                Assert.HasCount(2, result);
                Assert.IsTrue(result.Any(m => m.MediaResponse.id == mediaId1));
                Assert.IsTrue(result.Any(m => m.MediaResponse.id == mediaId2));
            }
        }
        [TestMethod]
        public async Task Handle_GetAllLikeForUser_ShouldReturnEmptyList()
        {
            var testUserId = Guid.NewGuid();

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var user = new UserModel(testUserId, "username", "password", "email");
                db.Users.Add(user);
                var newUserDetails = UserDetails.Create(testUserId, new Fullname("Jane", "Smith"), new Username("janesmith"), Email.Create("janesmith@example.com"));
                db.UsersDetails.Add(newUserDetails);
                await db.Context.SaveChangesAsync();
            }
            using (var scope2 = _serviceProvider.CreateScope())
            {
                var mediator = scope2.ServiceProvider.GetRequiredService<IMediator>();
                var command = new GetAllForUserQuery(testUserId);
                var result = await mediator.Send(command);
                Assert.HasCount(0, result);
            }
        }
        [TestMethod]
        public async Task Handle_GetAllLikedButUserDontExist_ShouldReturnEmptyList()
        {
            var nonExistentUserId = Guid.NewGuid();
            using (var scope2 = _serviceProvider.CreateScope())
            {
                var mediator = scope2.ServiceProvider.GetRequiredService<IMediator>();
                var command = new GetAllForUserQuery(nonExistentUserId);
                var result = await mediator.Send(command);
                Assert.HasCount(0, result);
            }
        }
    }
}