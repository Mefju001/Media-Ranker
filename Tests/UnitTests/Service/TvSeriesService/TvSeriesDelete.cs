using Application.Common.Interfaces;
using Application.Features.TvSeriesServices.DeleteById;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class TvSeriesDelete
    {
        private readonly Guid id = Guid.NewGuid();
        private AppDbContext appDbContext;
        private Mock<IMediator> mockMediator;
        private IMediaRepository<TvSeries> repository;
        private DeleteByIdHandler handler;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            repository = new MediaRepository<TvSeries>(appDbContext);
            mockMediator = new Mock<IMediator>();
            handler = new DeleteByIdHandler(repository, mockMediator.Object);
            await SeedData();
        }
        private async Task SeedData()
        {
            var genreId = Guid.NewGuid();
            var directorId = Guid.NewGuid();
            var genre = Genre.Create("Genre1", genreId);
            appDbContext.Genres.Add(genre);
            var movieInDb = TvSeries.Create("Title 1","desc", new Language("Lang"),new ReleaseDate(DateTime.UtcNow),genre.Id,2,30,"Netflix",EStatus.Unknown,id);
            appDbContext.Medias.Add(movieInDb);
            await appDbContext.SaveChangesAsync();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            appDbContext.Dispose();
        }
        [TestMethod]
        public async Task Handle_DeleteMovieById_ShouldDeleteMovieFromDb()
        {
            var result = await handler.Handle(new DeleteByIdCommand(id), CancellationToken.None);
            await appDbContext.SaveChangesAsync();

            var tvSeriesInDb = await appDbContext.Medias.FindAsync(id);
            Assert.IsNull(tvSeriesInDb, "Film powinien zostać usunięty z bazy danych.");

            mockMediator.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("Usunięto")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Handle_DeleteById_ShouldThrowNotFoundException()
        {
            var nonExistentMovieId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await handler.Handle(new DeleteByIdCommand(nonExistentMovieId), CancellationToken.None);
            });
        }
    }
}
