using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.ToWatch.DeleteById;
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
    public class DeleteByIdHandlerTests
    {
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        private Guid userId = Guid.NewGuid();
        private Guid gameId = Guid.NewGuid();
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
            services.AddValidatorsFromAssembly(typeof(DeleteByIdCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DeleteByIdHandler).Assembly);
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
            var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            appDbContext.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, new GameDetails("Developer A", "Engine A"), 3, new List<EPlatform>() { EPlatform.PC }, EGameStatus.Announced, true,gameId);
            appDbContext.Medias.Add(game);
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), "johndoe", Email.Create("johndoe@example.com"));
            appDbContext.UsersDetails.Add(user);
            var userModel = new UserModel(userId, "username", "password", "email");
            appDbContext.Users.Add(userModel);
            user.SetInteraction(gameId, null, ERatingVote.Liked);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task TestRemove_ShouldRemoveMediaFromWatchList()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new DeleteByIdCommand(gameId, userId), CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result.UserInteractions);
        }
        [TestMethod]
        public async Task TestRemove_ShouldThrowDomainException_WhenMediaIsNotInWatchList()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var nonExistentMediaId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<DomainException>(async()=>await mediator.Send(new DeleteByIdCommand(nonExistentMediaId, userId), CancellationToken.None));
            using var scope2 = _serviceProvider.CreateScope();
            var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result.UserInteractions);
        }
    }
}
