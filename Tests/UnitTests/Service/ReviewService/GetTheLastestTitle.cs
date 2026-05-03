using Application.Features.ReviewServices.GetTheLastestReview;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Tests.Service.ReviewService
{
    [TestClass]
    public class GetTheLastestTitle
    {
        private AppDbContext appDbContext;
        private GetTheLastestHandler GetTheLastestHandler;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            GetTheLastestHandler = new GetTheLastestHandler(appDbContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        private async Task SeedData()
        {
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            game.AddReview(Guid.NewGuid(), new Rating(4), "Good game!", new Username("testuser2"));
            game.AddReview(Guid.NewGuid(), new Rating(5), "Excellent game!", new Username("testuser2"));
            game.AddReview(Guid.NewGuid(), new Rating(5), "Excellent game!", new Username("testuser2"));
            game.AddReview(Guid.NewGuid(), new Rating(5), "Excellent game!", new Username("testuser2"));
            game.AddReview(Guid.NewGuid(), new Rating(5), "Excellent game!", new Username("testuser2"));
            appDbContext.Medias.Add(game);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task GetTheLastestTitle_Should_Return_List_Of_The_Lastest_Titles()
        {
            await SeedData();
            var result = await GetTheLastestHandler.Handle(new GetTheLastestQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(5, result);
        }
        [TestMethod]
        public async Task GetTheLastestTitle_Should_Return_Empty_List_Of_The_Lastest_Titles()
        {
            var result = await GetTheLastestHandler.Handle(new GetTheLastestQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
    }
}
