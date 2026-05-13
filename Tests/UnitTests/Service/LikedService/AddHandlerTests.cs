using Application.Behaviours;
using Application.Common.Interfaces;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;


namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class AddHandlerTests
    {
        private IServiceProvider _serviceProvider;
        private Guid _userId;
        private Guid _mediaId;
        [TestInitialize]
        public async Task setup()
        {
            var services = new ServiceCollection();

            // Baza danych
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
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
            await SeedData();
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = new UserModel(Guid.NewGuid(), "username", "password", "email");
            _userId = user.Id;
            var userDetails = UserDetails.Create(_userId, new Fullname("Name", "Surname"), new Username("username"), Email.Create("email@example.com"));

            db.Users.Add(user);
            db.UsersDetails.Add(userDetails);

            var genre = Genre.Create("Name");
            var game = Game.Create("Title", "Desc", new Language("Eng"), new ReleaseDate(DateTime.Now), genre.Id, "Dev", new List<EPlatform> { EPlatform.PC });
            _mediaId = game.Id;

            db.Genres.Add(genre);
            db.Medias.Add(game);

            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();
        }
        [TestMethod]
        public async Task Handle_AddLiked_ShouldAddLikedMedia()
        {
            using var scope = _serviceProvider.CreateScope();

            var testdb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var userInDb = await testdb.Users.FindAsync(_userId);
            var userDetailsInDb = await testdb.UsersDetails.FindAsync(_userId);
            if (userInDb == null || userDetailsInDb == null)
            {
                var count = await testdb.Users.CountAsync();
                Assert.Fail($"Użytkownik {_userId} nie istnieje! W bazie jest {count} osób.");
            }

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new AddCommand(_userId, _mediaId);


            var result = await mediator.Send(command);


            using var assertScope = _serviceProvider.CreateScope();
            var db = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();

            var likedMedia = await db.UserInteractions
                .FirstOrDefaultAsync(lm => lm.UserId == _userId && lm.MediaId == _mediaId);

            Assert.IsTrue(result);
            Assert.IsNotNull(likedMedia);
            Assert.AreEqual(_userId, likedMedia.UserId);
            Assert.AreEqual(_mediaId, likedMedia.MediaId);
        }

        [TestMethod]
        public async Task Handle_AddLikedWhereUserIdIsNull_ShouldThrowNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = new AddCommand(Guid.NewGuid(), _mediaId);

            await Assert.ThrowsExactlyAsync<NotFoundException>(() => mediator.Send(command));
        }
    }
}
