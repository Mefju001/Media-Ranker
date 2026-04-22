using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.MovieServices.MovieUpsert;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using Application.Notification;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class TvSeriesUpsert
    {
        private Guid tvSeriesId;
        private AppDbContext context;
        private IMediaRepository<TvSeries> repository;
        private Mock<IMediator> mediatorMock;
        private GenreHelperService genreHelperService;
        private TvSeriesUpsertHandler handler;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            genreHelperService = new GenreHelperService(new GenreRepository(context));
            repository = new MediaRepository<TvSeries>(context);
            mediatorMock = new Mock<IMediator>();
            handler = new TvSeriesUpsertHandler(genreHelperService, mediatorMock.Object, repository);
            await SeedData();
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        private async Task SeedData()
        {
            var genre = Genre.Create("Action", Guid.NewGuid());
            context.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            context.Genres.Add(genre2);
            var tvSeries = TvSeries.Create("Title", "desc", new Language("Lang"), new ReleaseDate(DateTime.UtcNow), genre.Id, 2, 20, "Netflix", EStatus.Continuing);
            context.Medias.Add(tvSeries);
            tvSeriesId = tvSeries.Id;
            context.SaveChanges();
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNull_ShouldCreateNewTvSeries()
        {
            var command = new UpsertTvSeriesCommand(
                null,
                "New Title",
                "desc",
                new GenreRequest("name"),
                new ReleaseDate(DateTime.UtcNow),
                "Lang",
                2,
                20,
                "Netflix",
                EStatus.Continuing
                );


            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var tvSeriesInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Title");
            Assert.IsNotNull(tvSeriesInDb);
            Assert.AreEqual("New Title", tvSeriesInDb.Title);

            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_WhenIdIsNotNull_ShouldUpdateExistingTvSeries()
        {
            var command = new UpsertTvSeriesCommand(
                            tvSeriesId,
                            "New Title",
                            "Description",
                            new GenreRequest("Adventure"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            EStatus.Continuing
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Title");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Title", gameInDb.Title);
            Assert.AreEqual("Description", gameInDb.Description);
            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("zaktualizowany")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Handle_WhenGenreDoesNotExist_ShouldCreateNewGenre()
        {
            var command = new UpsertTvSeriesCommand(
                null,
                "New Title",
                "Description",
                new GenreRequest("New Genre"),
                new ReleaseDate(DateTime.UtcNow),
                "Lang",
                2,
                20,
                "Netflix",
                EStatus.Continuing
                );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Name.Value == "New Genre");
            Assert.IsNotNull(genreInDb);
            Assert.AreEqual("New Genre", genreInDb.Name.Value);
        }
        [TestMethod]
        public async Task Handle_GenreRequestIsEmpty_ShouldThrowArgumentException()
        {
            var command = new UpsertTvSeriesCommand(
                            null,
                            "New Title",
                            "Description",
                            new GenreRequest(string.Empty),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            EStatus.Continuing
                            );
            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_WhenTvSeriesDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new UpsertTvSeriesCommand(
                            Guid.NewGuid(),
                            "New Title",
                            "Description",
                            new GenreRequest("Action"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            EStatus.Continuing
                            );
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ChangeGenreToExisting_ShouldUpdateTvSeriesGenre()
        {
            var command = new UpsertTvSeriesCommand(
                            tvSeriesId,
                            "New Title",
                            "Description",
                            new GenreRequest("Adventure"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            EStatus.Continuing
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var tvSeriesInDb = await context.Medias.FirstOrDefaultAsync(g => g.Id == tvSeriesId);
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Id == tvSeriesInDb.GenreId);
            Assert.IsNotNull(tvSeriesInDb);
            Assert.AreEqual("Adventure", genreInDb.Name.Value);
        }
    }
}
