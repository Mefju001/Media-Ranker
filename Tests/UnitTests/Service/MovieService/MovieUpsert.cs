

using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.GamesServices.GameUpsert;
using Application.Features.MovieServices.MovieUpsert;
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

namespace Tests.Service.MovieService
{
    [TestClass]
    public class MovieUpsert
    {
        private Guid MovieId;
        private AppDbContext context;
        private IMediaRepository<Movie> repository;
        private Mock<IMediator> mediatorMock;
        private GenreHelperService genreHelperService;
        private DirectorHelperService directorHelperService;
        private MovieUpsertHandler handler;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            genreHelperService = new GenreHelperService(new GenreRepository(context));
            directorHelperService = new DirectorHelperService(new DirectorRepository(context));
            repository = new MediaRepository<Movie>(context);
            mediatorMock = new Mock<IMediator>();
            handler = new MovieUpsertHandler(directorHelperService, genreHelperService, mediatorMock.Object, repository);
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
            var director = Director.Create("Name", "Surname", Guid.NewGuid());
            context.Directors.Add(director);
            var movie = Movie.Create("Title A", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, director.Id, new Duration(TimeSpan.FromMinutes(120)), true);
            context.Medias.Add(movie);
            MovieId = movie.Id;
            context.SaveChanges();
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNull_ShouldCreateNewMovie()
        {
            var command = new UpsertMovieCommand(
                null,
                "New Movie",
                "Description",
                new GenreRequest("Action"),
                new DirectorRequest("Name", "Surname"),
                DateTime.UtcNow,
                "EN",
                TimeSpan.FromMinutes(120),
                false
                );


            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var movieInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Movie");
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual("New Movie", movieInDb.Title);

            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [TestMethod]
        public async Task Handle_WhenIdIsNotNull_ShouldUpdateExistingMovie()
        {
            var command = new UpsertMovieCommand(
                            MovieId,
                            "New Movie",
                            "Description",
                            new GenreRequest("Action"),
                            new DirectorRequest("Name", "Surname"),
                            DateTime.UtcNow,
                            "EN",
                            TimeSpan.FromMinutes(120),
                            false
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Movie");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Movie", gameInDb.Title);
            Assert.AreEqual("Description", gameInDb.Description);
            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("zaktualizowany")),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Handle_WhenGenreDoesNotExist_ShouldCreateNewGenre()
        {
            var command = new UpsertMovieCommand(
                null,
                "New Movie",
                "Description",
                new GenreRequest("New Genre"),
                new DirectorRequest("Name", "Surname"),
                DateTime.UtcNow,
                "EN",
                TimeSpan.FromMinutes(120),
                false
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
            var command = new UpsertMovieCommand(
                            null,
                            "New Movie",
                            "Description",
                            new GenreRequest(string.Empty),
                            new DirectorRequest("Name", "Surname"),
                            DateTime.UtcNow,
                            "EN",
                            TimeSpan.FromMinutes(120),
                            false
                            );
            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_WhenMovieDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new UpsertMovieCommand(
                            Guid.NewGuid(),
                            "New Movie",
                            "Description",
                            new GenreRequest("Action"),
                            new DirectorRequest("Name", "Surname"),
                            DateTime.UtcNow,
                            "EN",
                            TimeSpan.FromMinutes(120),
                            false
                            );
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ChangeGenreToExisting_ShouldUpdateMovieGenre()
        {
            var command = new UpsertMovieCommand(
                            MovieId,
                            "New Movie",
                            "Description",
                            new GenreRequest("Action"),
                            new DirectorRequest("Name", "Surname"),
                            DateTime.UtcNow,
                            "EN",
                            TimeSpan.FromMinutes(120),
                            false
                            );
            var result = await handler.Handle(command, CancellationToken.None);
            //because pipeline in real app will save changes after handler execution
            await context.SaveChangesAsync();
            var movieInDb = await context.Medias.FirstOrDefaultAsync(g => g.Id == MovieId);
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Id == movieInDb.GenreId);
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual("Action", genreInDb.Name.Value);
        }
    }
}
