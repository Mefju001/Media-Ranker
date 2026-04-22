using Application.Common.Interfaces;
using Application.Features.MovieServices.DeleteById;
using Application.Notification;
using Domain.Aggregate;
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

namespace Tests.Service.MovieService
{
    [TestClass]
    public class MovieDelete
    {
        private readonly Guid id = Guid.NewGuid();
        private AppDbContext appDbContext;
        private Mock<IMediator> mockMediator;
        private IMediaRepository<Movie> movieRepository;
        private DeleteByIdHandler handler;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            movieRepository = new MediaRepository<Movie>(appDbContext);
            mockMediator = new Mock<IMediator>();
            handler = new DeleteByIdHandler(movieRepository, mockMediator.Object);
            await SeedData();
        }
        private async Task SeedData()
        {
            var genreId = Guid.NewGuid();
            var directorId = Guid.NewGuid();
            var genre = Genre.Create("Genre1", genreId);
            appDbContext.Genres.Add(genre);
            var director = Director.Create("Director1", "Director1", directorId);
            appDbContext.Directors.Add(director);
            var movieInDb = Movie.Create("Test Movie", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), genreId, directorId, new Duration(TimeSpan.FromMinutes(120)), true,id);
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

            var movieInDb = await appDbContext.Medias.FindAsync(id);
            Assert.IsNull(movieInDb, "Film powinien zostać usunięty z bazy danych.");

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
