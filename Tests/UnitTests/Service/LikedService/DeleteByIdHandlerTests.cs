using Application.Features.Liked.DeleteById;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class DeleteByIdHandlerTests
    {
        private Guid userId;
        private Guid mediaId;
        private DeleteByIdHandler handler;
        private AppDbContext context;
        private ILogger<DeleteByIdHandler> logger;
        private IUserDetailsRepository userDetailsRepository;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            logger = new Mock<ILogger<DeleteByIdHandler>>().Object;
            userDetailsRepository = new UserDetailsRepository(context);
            handler = new DeleteByIdHandler(userDetailsRepository, logger);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        private async Task SeedData()
        {
            var user = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            userId = user.Id;
            var genre = Genre.Create("Name");
            var game = Game.Create("Title", "Desc", new Language("Eng"), new ReleaseDate(DateTime.Now), genre.Id, "Dev", new List<EPlatform> { EPlatform.PC });
            mediaId = game.Id;
            user.SetInteraction(mediaId, null, ERatingVote.Liked);
            context.UsersDetails.Add(user);
            await context.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_DeleteLikedMedia_ShouldDelete()
        {
            var initialCheck = await context.UsersDetails
                .Include(u => u.UserInteractions)
                .FirstOrDefaultAsync(u => u.Id == userId);
            Assert.IsTrue(initialCheck.UserInteractions.Any(m => m.MediaId == mediaId), "Dane powinny istnieć przed usunięciem");
            var command = new DeleteByIdCommand(userId, mediaId);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(result);
            var user = await context.UsersDetails
                .Include(u => u.UserInteractions)
                .FirstOrDefaultAsync(u => u.Id == userId);
            Assert.IsNotNull(user);
            Assert.IsFalse(user.UserInteractions.Any(m => m.MediaId == mediaId), "Media powinno zostać usunięte z ulubionych");
        }
        [TestMethod]
        public async Task Handle_DeleteLikedMedia_UserNotFound_ShouldThrow()
        {
            var nonExistentUserId = Guid.NewGuid();
            var command = new DeleteByIdCommand(nonExistentUserId, mediaId);
            await Assert.ThrowsExactlyAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_DeleteLikedMediaWhichIsNotLiked_ShouldThrowDomainException()
        {
            var nonLikedMediaId = Guid.NewGuid();
            var command = new DeleteByIdCommand(userId, nonLikedMediaId);
            await Assert.ThrowsExactlyAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
