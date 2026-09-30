using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Statuses.SetStatus;
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

namespace Tests.Service.StatusesService
{
    [TestClass]
    public class AddHandlerTests
    {
        private Guid gameId = Guid.NewGuid(), userId, game2Id = Guid.NewGuid();
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
            services.AddValidatorsFromAssembly(typeof(SetStatusCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(SetStatusHandler).Assembly);
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
            var game = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, new GameDetails("developer","Engine"), 3, new List<EPlatform>() { EPlatform.PC }, EGameStatus.Announced, true, gameId);
            var game2 = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, new GameDetails("developer","Engine"), 3, new List<EPlatform>() { EPlatform.PC }, EGameStatus.Announced, false, game2Id);
            appDbContext.Medias.Add(game);
            appDbContext.Medias.Add(game2);
            var userModel = new UserModel(Guid.NewGuid(), "username", "password", "email");
            userId = userModel.Id;
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), "johndoe", Email.Create("johndoe@example.com"));
            appDbContext.UsersDetails.Add(user);
            appDbContext.Users.Add(userModel);
            user.SetTypeInteractions(gameId, ETypeInteractions.Completed);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task TestAdd_ShouldAddMediaToUserInteractions()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SetStatusCommand(userId, game2Id, ERatingVote.Liked,ETypeInteractions.Completed ), CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var appDbContext = scope2.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result.UserInteractions);
            Assert.IsTrue(result.UserInteractions.Any(ui => ui.MediaId == game2Id && ui.TypeInteractions == ETypeInteractions.Completed && ui.RatingVote == ERatingVote.Liked));
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
                await mediator.Send(new SetStatusCommand(invalidMediaId, userId, ERatingVote.Liked, ETypeInteractions.Completed), CancellationToken.None);
            });
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            { 
                await mediator.Send(new SetStatusCommand(gameId, invalidUserId, ERatingVote.Liked, ETypeInteractions.Completed), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task TestAdd_ShouldNotAddDuplicateMedia()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await mediator.Send(new SetStatusCommand(gameId, userId, ERatingVote.Liked, ETypeInteractions.Completed), CancellationToken.None);
            });            
        }

    }
}
