using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.ToWatch.Add;
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

namespace Tests.Service.ToWatchService
{
    [TestClass]
    public class AddHandlerTests
    {
        private Guid gameId = Guid.NewGuid(), userId, game2Id;
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;

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
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            appDbContext.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, "developer", new List<EPlatform>() { EPlatform.PC }, gameId);
            var game2 = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, "developer", new List<EPlatform>() { EPlatform.PC });
            appDbContext.Medias.Add(game);
            appDbContext.Medias.Add(game2);
            game2Id = game2.Id;
            var userModel = new UserModel(Guid.NewGuid(), "username", "password", "email");
            userId = userModel.Id;
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), "johndoe", Email.Create("johndoe@example.com"));
            appDbContext.UsersDetails.Add(user);
            appDbContext.Users.Add(userModel);
            user.SetTypeInteractions(gameId, ETypeInteractions.WANT_TO_WATCH);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task TestAdd_ShouldAddMediaToWatchList()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new AddCommand(game2Id, userId ), CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var appDbContext = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result.UserInteractions);
            Assert.AreEqual(gameId, result.UserInteractions.Last().MediaId);
            Assert.AreEqual(game2Id,result.UserInteractions.First().MediaId );
        }
        [TestMethod]
        public async Task TestAdd_ShouldThrowNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var invalidMediaId = Guid.NewGuid();
            var invalidUserId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await mediator.Send(new AddCommand(invalidMediaId, userId), CancellationToken.None);
            });
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            { 
                await mediator.Send(new AddCommand(gameId, invalidUserId), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task TestAdd_ShouldNotAddDuplicateMedia()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsExactlyAsync<DomainException>(async () =>
            {
                await mediator.Send(new AddCommand(gameId, userId), CancellationToken.None);
            });            
        }

    }
}
