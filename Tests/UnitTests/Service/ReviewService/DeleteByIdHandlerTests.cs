using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Reviews.DeleteById;
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
    public class DeleteByIdHandlerTests
    {
        private Guid mediaId;
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
            services.AddValidatorsFromAssembly(typeof(DeleteByIdCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DeleteByIdHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Media>, MediaRepository<Media>>();
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
            var userModel = new UserModel(Guid.NewGuid(),"username", "password","email@example.com");
            appDbContext.Users.Add(userModel);
            var user = UserDetails.Create(userModel.Id, new Fullname("testuser", "testuser"), new Username("testuser"), Email.Create("testuser@example.com"));
            appDbContext.UsersDetails.Add(user);
            secondUserId = userModel.Id;
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            mediaId = game.Id;
            game.AddReview(secondUserId, new Rating(4), "Good game!", new Username("testuser2"));
            reviewId = game.Reviews.First(r => r.UserId == secondUserId).Id;
            appDbContext.Medias.Add(game);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task DeleteReview_ShouldDeleteReview()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new DeleteByIdCommand(mediaId, reviewId);
            var result = await mediator.Send(command, CancellationToken.None);
            
            using var scope2 = _serviceProvider.CreateScope();
            var mediaRepository = scope2.ServiceProvider.GetRequiredService<IMediaRepository<Media>>();
            var media = await mediaRepository.GetByIdAsync(mediaId, CancellationToken.None);
            Assert.IsTrue(result);
            Assert.IsFalse(media.Reviews.Any(r => r.Id == reviewId));
        }
        [TestMethod]
        public async Task DeleteReviewWhereMediaDoesNotExist_ShouldThrowNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new DeleteByIdCommand(Guid.NewGuid(), reviewId);
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task DeleteReviewWhereReviewDoesNotExist_ShouldThrowDomainException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new DeleteByIdCommand(mediaId, Guid.NewGuid());
            await Assert.ThrowsExactlyAsync<DomainException>(async () => await mediator.Send(command, CancellationToken.None));
        }
    }
}
