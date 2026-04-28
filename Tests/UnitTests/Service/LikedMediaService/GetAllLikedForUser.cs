using Application.Features.LikedServices.GetAllLikedByUser;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.LikedMediaService
{
    [TestClass]
    public class GetAllLikedForUser
    {
        private GetAllLikedByUserHandler handler;
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
            handler = new GetAllLikedByUserHandler(appDbContext);
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
            userDetails.AddLikedMedia(game.Id);
            userDetails.AddLikedMedia(game2.Id);
            appDbContext.Medias.Add(game2); await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_GetAllLikedForUser_ShouldReturnListOfMedias()
        {
            var result = await handler.Handle(new GetAllLikedByUserQuery(userId), CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.IsTrue(result.Any(m => m.Media.id == mediaId1));
            Assert.IsTrue(result.Any(m => m.Media.id == mediaId2));
        }
        [TestMethod]
        public async Task Handle_GetAllLikeForUser_ShouldReturnEmptyList()
        {
            var newUserDetails = UserDetails.Create(null, new Fullname("Jane", "Smith"), new Username("janesmith"), Email.Create("janesmith@example.com"));
            appDbContext.UsersDetails.Add(newUserDetails);
            await appDbContext.SaveChangesAsync();
            var result = await handler.Handle(new GetAllLikedByUserQuery(newUserDetails.Id), CancellationToken.None);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_GetAllLikedButUserDontExist_ShouldReturnEmptyList()
        {
            var nonExistentUserId = Guid.NewGuid();
            var result = await handler.Handle(new GetAllLikedByUserQuery(nonExistentUserId), CancellationToken.None);
            Assert.HasCount(0, result);
        }
    }
}
