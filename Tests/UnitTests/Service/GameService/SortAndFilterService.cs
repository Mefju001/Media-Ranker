using Application.Features.Games.GetByCriteria;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Tests.Service.GameService
{
    [TestClass ]
    public class SortAndFilterService
    {
        private AppDbContext appDbContext;
        private ISortAndFilterService service;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            service = new Application.Features.Games.GetByCriteria.SortAndFilterService(appDbContext);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        private async Task SeedData()
        {
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game B", "Description B", new Language("English"),new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, "Developer B",new List<EPlatform>() { EPlatform.PC});
            appDbContext.Medias.Add(game);
            var game2 = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            appDbContext.Medias.Add(game2);
            appDbContext.SaveChanges();
        }


        [TestMethod]
        public async Task GetGamesByCriteria_WhenFilterByTitle_ShouldReturnMatch() 
        {
            var query = new GetByCriteriaQuery("Game A", null, null, null, null, null,null,true);
            var result = await service.GetByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(1, result);
            Assert.AreEqual("Game A", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered() 
        {
            var query = new GetByCriteriaQuery(null, null, null, null, null, null, "Date", true);
            var result = await service.GetByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            var query = new GetByCriteriaQuery(null, null, null, null, null, null, null, true); var result = await service.GetByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
    }
}
