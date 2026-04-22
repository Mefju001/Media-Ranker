using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetMoviesByCriteria;
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

namespace Tests.Service.MovieService
{
    [TestClass]
    public class MovieSortAndFilterService
    {
        private AppDbContext appDbContext;
        private IMovieSortAndFilterService service;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            service = new Application.Features.MovieServices.GetMoviesByCriteria.MovieSortAndFilterService(appDbContext);
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
            var director = Director.Create("Director1","Director1",Guid.NewGuid());
            appDbContext.Directors.Add(director);
            var director2 = Director.Create("Director2", "Director2", Guid.NewGuid());
            appDbContext.Directors.Add(director2);
            var movie = Movie.Create("Title A", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, director.Id, new Duration(TimeSpan.FromMinutes(120)),true);
            appDbContext.Medias.Add(movie);
            var movie2 = Movie.Create("Title B", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-15)), genre2.Id, director2.Id, new Duration(TimeSpan.FromMinutes(120)), true);
            appDbContext.Medias.Add(movie2);
            appDbContext.SaveChanges();
        }


        [TestMethod]
        public async Task GetGamesByCriteria_WhenFilterByTitle_ShouldReturnMatch()
        {
            var query = new GetMoviesByCriteriaQuery
            {
                TitleSearch = "Title A"
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(1, result);
            Assert.AreEqual("Title A", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered()
        {
            var query = new GetMoviesByCriteriaQuery
            {
                SortByField = "Date",
                IsDescending = true
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title A", result[0].Title);
            Assert.AreEqual("Title B", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            var query = new GetMoviesByCriteriaQuery
            {
            };
            var result = await service.Handler(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title A", result[0].Title);
            Assert.AreEqual("Title B", result[1].Title);
        }
    }
}
