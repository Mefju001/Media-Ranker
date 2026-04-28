using Application.Common.Interfaces;
using Application.Features.LikedServices.AddLiked;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;


namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class AddLikedMedia
    {
        private Guid userId;
        private Guid mediaId;
        private AddLikedHandler handler;
        private AppDbContext appDbContext;
        private IMediaRepository<Media> mediaRepository;
        private IUserDetailsRepository userDetailsRepository;
        private Mock<ILogger<AddLikedHandler>> logger;
        [TestInitialize]
        public async Task setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                           .Options;
            appDbContext = new AppDbContext(options);
            logger = new Mock<ILogger<AddLikedHandler>>();
            mediaRepository = new MediaRepository<Media>(appDbContext);
            userDetailsRepository = new UserDetailsRepository(appDbContext);
            handler = new AddLikedHandler(logger.Object, mediaRepository, userDetailsRepository);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        private async Task SeedData()
        {
            var user = new UserModel(Guid.NewGuid(), "username", "password", "email");
            userId = user.Id;
            var userDetails = UserDetails.Create(userId, new Fullname("Name", "Surname"), new Username("username"), Email.Create("email@example.com"));
            appDbContext.Users.Add(user);
            appDbContext.UsersDetails.Add(userDetails);
            var genre = Genre.Create("Name");
            var game = Game.Create("Title", "Desc", new Language("Eng"), new ReleaseDate(DateTime.Now), genre.Id, "Dev", new List<EPlatform> { EPlatform.PC });
            mediaId = game.Id;
            appDbContext.Genres.Add(genre);
            appDbContext.Medias.Add(game);
            await appDbContext.SaveChangesAsync();

        }
        [TestMethod]
        public async Task Handle_AddLiked_ShouldAddLikedMedia()
        {
            var command = new AddLikedCommand(userId, mediaId);

            var result = await handler.Handle(command, CancellationToken.None);
            await appDbContext.SaveChangesAsync();
            var likedMedia = await appDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == userId && lm.MediaId == mediaId);
            Assert.IsTrue(result);
            Assert.IsNotNull(likedMedia);
            Assert.AreEqual(likedMedia.UserId, userId);
            Assert.AreEqual(likedMedia.MediaId, mediaId);
        }
        [TestMethod]
        public async Task Handle_AddLikedWhereUserIdIsNull_ShouldThrowNotFoundException()
        {
            var command = new AddLikedCommand(Guid.NewGuid(), mediaId);
            await Assert.ThrowsExactlyAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_AddLikedWhereMediaIdIsNull_ShouldThrowNotFoundException()
        {
            var command = new AddLikedCommand(userId, Guid.NewGuid());
            await Assert.ThrowsExactlyAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
