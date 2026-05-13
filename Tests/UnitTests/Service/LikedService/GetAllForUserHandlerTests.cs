using Application.Features.Liked.GetAllForUser;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class GetAllForUserHandlerTests
    {
        private GetAllForUserHandler handler;
        private AppDbContext appDbContext;
        private Guid mediaId1;
        private Guid mediaId2;
        private Guid userId;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            handler = new GetAllForUserHandler(appDbContext);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        private async Task SeedData()
        {
            var userDetails = UserDetails.Create(null, new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            userId = userDetails.Id;
            appDbContext.UsersDetails.Add(userDetails);
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game B", "Description B", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, "Developer B", new List<EPlatform>() { EPlatform.PC });
            mediaId1 = game.Id;
            appDbContext.Medias.Add(game);
            var game2 = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            mediaId2 = game2.Id;
            userDetails.SetInteraction(game.Id, null, ERatingVote.Liked);
            userDetails.SetInteraction(game2.Id, null, ERatingVote.Liked);
            appDbContext.Medias.Add(game2); 
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_GetAllLikedForUser_ShouldReturnListOfMedias()
        {
            var result = await handler.Handle(new GetAllForUserQuery(userId), CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.IsTrue(result.Any(m => m.MediaResponse.id == mediaId1));
            Assert.IsTrue(result.Any(m => m.MediaResponse.id == mediaId2));
        }
        [TestMethod]
        public async Task Handle_GetAllLikeForUser_ShouldReturnEmptyList()
        {
            var newUserDetails = UserDetails.Create(null, new Fullname("Jane", "Smith"), new Username("janesmith"), Email.Create("janesmith@example.com"));
            appDbContext.UsersDetails.Add(newUserDetails);
            await appDbContext.SaveChangesAsync();
            var result = await handler.Handle(new GetAllForUserQuery(newUserDetails.Id), CancellationToken.None);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_GetAllLikedButUserDontExist_ShouldReturnEmptyList()
        {
            var nonExistentUserId = Guid.NewGuid();
            var result = await handler.Handle(new GetAllForUserQuery(nonExistentUserId), CancellationToken.None);
            Assert.HasCount(0, result);
        }
    }
}
