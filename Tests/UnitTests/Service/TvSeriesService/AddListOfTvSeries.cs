using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.MovieServices.AddListOfMovies;
using Application.Features.TvSeriesServices.AddListOfTvSeries;
using Application.Notification;
using Domain.Aggregate;
using Domain.Enums;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class AddListOfTvSeries
    {
        private AppDbContext context;
        private AddListOfTvSeriesHandler handler;
        private IMediaRepository<TvSeries> repository;
        private Mock<IMediator> mediatorMock;
        private IGenreHelperService genreHelperService;
        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            mediatorMock = new Mock<IMediator>();
            repository = new MediaRepository<TvSeries>(context);
            genreHelperService = new GenreHelperService(new GenreRepository(context));
            handler = new AddListOfTvSeriesHandler(mediatorMock.Object, genreHelperService, repository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        [TestMethod]
        public async Task Handle_AddTwoTvSeries_ShouldCreateTwoTvSeries()
        {
            var listOfTvSeries = new List<TvSeriesRequest>
            {
                new TvSeriesRequest
                (
                    "Title 1",
                    "Desc 1",
                    new GenreRequest("Name 1"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.Continuing
                ),
                new TvSeriesRequest
                (
                    "Title 2",
                    "Desc 2",
                    new GenreRequest("Name 2"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.EndedOrRemoved
                )
            };
            var command = new AddListOfTvSeriesCommand(listOfTvSeries);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
            var moviesInDb = await context.Medias.ToListAsync();
            Assert.IsNotNull(moviesInDb);
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Title 1"));
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Title 2"));
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            var command = new AddListOfTvSeriesCommand(new List<TvSeriesRequest>());
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_AddTvSeriesWithExistingGenre_ShouldCreateTvSeriesWithExistingGenre()
        {
            var existingGenre = Genre.Create("Existing Genre", Guid.NewGuid());
            context.Genres.Add(existingGenre);
            await context.SaveChangesAsync();
            var listOfTvSeries = new List<TvSeriesRequest>
            {
                new TvSeriesRequest
                (
                    "Title 1",
                    "Desc 1",
                    new GenreRequest("Existing Genre"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.Continuing
                )
            };
            var command = new AddListOfTvSeriesCommand(listOfTvSeries);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            var movieInDb = await context.Medias.FirstOrDefaultAsync(m => m.Title == "Title 1");
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual(existingGenre.Id, movieInDb.GenreId);
        }
        [TestMethod]
        public async Task Handle_OneTvSeriesInvalid_ShouldThrowExceptionAndAddNothing()
        {
            var listOfTvSeries = new List<TvSeriesRequest>
            {
                new TvSeriesRequest
                (
                    "Title 1",
                    "Desc 1",
                    new GenreRequest("Name 1"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.Continuing
                ),
                new TvSeriesRequest
                (
                    string.Empty,
                    "Desc 2",
                    new GenreRequest("Name 2"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.EndedOrRemoved
                )
            };
            var command = new AddListOfTvSeriesCommand(listOfTvSeries);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(command, CancellationToken.None));
            await context.SaveChangesAsync();
            var count = await context.Medias.CountAsync();
            Assert.AreEqual(0, count);

        }
        [TestMethod]
        public async Task Handle_MultipleTvSeriesWithSameNewGenre_ShouldCreateOnlyOneGenre()
        {
            var listOfTvSeries = new List<TvSeriesRequest>
            {
                new TvSeriesRequest
                (
                    "Title 1",
                    "Desc 1",
                    new GenreRequest("Name 1"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.Continuing
                ),
                new TvSeriesRequest
                (
                    "Title 2",
                    "Desc 2",
                    new GenreRequest("Name 1"),
                    DateTime.UtcNow,
                    "Language",
                    3,
                    20,
                    "Netflix",
                    EStatus.EndedOrRemoved
                )
            };
            var command = new AddListOfTvSeriesCommand(listOfTvSeries);

            await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();

            var genresInDb = await context.Genres.Where(g => g.Name.Value == "Name 1").ToListAsync();
            Assert.AreEqual(1, genresInDb.Count, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
        }

    }
}
