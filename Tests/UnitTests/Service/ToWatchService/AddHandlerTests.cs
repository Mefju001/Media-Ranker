using Application.Common.Interfaces;
using Application.Features.ToWatch.Add;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Tests.Service.ToWatchService
{
    [TestClass]
    public class AddHandlerTests
    {
        private Guid gameId = Guid.NewGuid(), userId = Guid.NewGuid();
        private AddHandler handler;
        private IMediaRepository<Media> mediaRepository;
        private IUserDetailsRepository userDetailsRepository;
        private AppDbContext appDbContext;

        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            mediaRepository = new MediaRepository<Media>(appDbContext);
            userDetailsRepository = new UserDetailsRepository(appDbContext);
            handler = new AddHandler(mediaRepository, userDetailsRepository);
            await SeedData();
        }
        private async Task SeedData()
        {
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            appDbContext.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), genreId, "developer", new List<EPlatform>() { EPlatform.PC }, gameId);
            appDbContext.Medias.Add(game);
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            appDbContext.UsersDetails.Add(user);
            await appDbContext.SaveChangesAsync();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        [TestMethod]
        public async Task TestAdd_ShouldAddMediaToWatchList()
        {
            await handler.Handle(new AddCommand(gameId, userId ), CancellationToken.None);
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result.UserInteractions);
            Assert.AreEqual(gameId, result.UserInteractions.First().Id.MediaId);
        }
        [TestMethod]
        public async Task TestAdd_ShouldThrowNotFoundException()
        {
            var invalidMediaId = Guid.NewGuid();
            var invalidUserId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await handler.Handle(new AddCommand(invalidMediaId, userId), CancellationToken.None);
            });
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            { 
                await handler.Handle(new AddCommand(gameId, invalidUserId), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task TestAdd_ShouldNotAddDuplicateMedia()
        {
            await handler.Handle(new AddCommand(gameId, userId), CancellationToken.None);
            await Assert.ThrowsExactlyAsync<DomainException>(async () =>
            {
                await handler.Handle(new AddCommand(gameId, userId), CancellationToken.None);
            });            
        }

    }
}
