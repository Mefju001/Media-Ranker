using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class TvSeriesSortAndFilterService
    {
        private AppDbContext appDbContext;
        private ITvSeriesSortAndFilterService service;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            service = new Application.Features.TvSeriesServices.GetTvSeriesByCriteria.TvSeriesSortAndFilterService(appDbContext);
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
            var tvSeries = TvSeries.Create("Title 1","desc",new Language("Lang"), new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, 2, 20, "Netflix", EStatus.Upcoming);
            appDbContext.Medias.Add(tvSeries);
            var tvSeries2 = TvSeries.Create("Title 2", "desc", new Language("Lang"), new ReleaseDate(DateTime.UtcNow.AddDays(-15)), genre2.Id, 2, 20, "Netflix", EStatus.Upcoming);
            appDbContext.Medias.Add(tvSeries2);
            appDbContext.SaveChanges();
        }


        [TestMethod]
        public async Task GetGamesByCriteria_WhenFilterByTitle_ShouldReturnMatch()
        {
            var query = new GetTvSeriesByCriteriaQuery
            {
                TitleSearch = "Title 1"
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(1, result);
            Assert.AreEqual("Title 1", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered()
        {
            var query = new GetTvSeriesByCriteriaQuery
            {
                SortByField = "Date",
                IsDescending = true
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title 1", result[0].Title);
            Assert.AreEqual("Title 2", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            var query = new GetTvSeriesByCriteriaQuery
            {
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title 1", result[0].Title);
            Assert.AreEqual("Title 2", result[1].Title);
        }
    }
}
