using Application.Features.GamesServices.GetGamesByCriteria;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Tests.Service.GameService
{
    [TestClass ]
    public class GameSortAndFilterService
    {
        private AppDbContext appDbContext;
        private IGameSortAndFilterService service;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            service = new Application.Features.GamesServices.GetGamesByCriteria.GameSortAndFilterService(appDbContext);
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
            var query = new GetGamesByCriteriaQuery
            {
                title = "Game A"
            };
            var result = await service.GetGamesByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(1, result);
            Assert.AreEqual("Game A", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered() 
        {
            var query = new GetGamesByCriteriaQuery
            {
                sortByField = "Date",
                IsDescending = true
            };
            var result = await service.GetGamesByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            var query = new GetGamesByCriteriaQuery
            {
            };
            var result = await service.GetGamesByCriteriaAsync(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Game A", result[0].Title);
            Assert.AreEqual("Game B", result[1].Title);
        }
    }
}
