using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Features.MovieServices.AddListOfMovies;
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

namespace Tests.Service.MovieService
{
    [TestClass]
    public class AddListOfMovies
    {
        private AppDbContext context;
        private AddListOfMoviesHandler handler;
        private IMediaRepository<Movie> movieRepository;
        private Mock<IMediator> mediatorMock;
        private IGenreHelperService genreHelperService;
        private IDirectorHelperService directorHelperService;
        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            mediatorMock = new Mock<IMediator>();
            movieRepository = new MediaRepository<Movie>(context);
            genreHelperService = new GenreHelperService(new GenreRepository(context));
            directorHelperService = new DirectorHelperService(new DirectorRepository(context));
            handler = new AddListOfMoviesHandler(movieRepository, genreHelperService, mediatorMock.Object, directorHelperService);
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        [TestMethod]
        public async Task Handle_AddTwoMovies_ShouldCreateTwoMovies()
        {
            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                ("Movie 1",
                "Description 1",
                new GenreRequest("Genre 1"),
                new DirectorRequest("Director 1", "Director 1"),
                DateTime.UtcNow,
                "English",
                TimeSpan.FromHours(2),
                true
                ),
                new MovieRequest
                (
                    "Movie 2",
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    new DirectorRequest("Director 1", "Director 1"),
                    DateTime.UtcNow,
                    "English",
                    TimeSpan.FromHours(2),
                    true
                )
            };
            var command = new AddListOfMoviesCommand(listOfMovies);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            mediatorMock.Verify(m => m.Publish(
                It.Is<LogNotification>(n => n.Message.Contains("dodana")),
                It.IsAny<CancellationToken>()), Times.Once);
            var moviesInDb = await context.Medias.ToListAsync();
            Assert.IsNotNull(moviesInDb);
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 1"));
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 2"));
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            var command = new AddListOfMoviesCommand(new List<MovieRequest>());
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_AddMovieWithExistingGenre_ShouldCreateMovieWithExistingGenre()
        {
            var existingGenre = Genre.Create("Existing Genre", Guid.NewGuid());
            context.Genres.Add(existingGenre);
            await context.SaveChangesAsync();
            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                ("Movie 1",
                "Description 1",
                new GenreRequest("Existing Genre"),
                new DirectorRequest("Director 1", "Director 1"),
                DateTime.UtcNow,
                "English",
                TimeSpan.FromHours(2),
                true
                )
            };
            var command = new AddListOfMoviesCommand(listOfMovies);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            var movieInDb = await context.Medias.FirstOrDefaultAsync(m => m.Title == "Movie 1");
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual(existingGenre.Id, movieInDb.GenreId);
        }
        [TestMethod]
        public async Task Handle_OneMovieInvalid_ShouldThrowExceptionAndAddNothing()
        {
            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                ("Movie 1",
                "Description 1",
                new GenreRequest("Genre 1"),
                new DirectorRequest("Director 1", "Director 1"),
                DateTime.UtcNow,
                "English",
                TimeSpan.FromHours(2),
                true
                ),
                new MovieRequest
                (
                    string.Empty, // Invalid title
                    "Description 2",
                    new GenreRequest("Genre 2"),
                    new DirectorRequest("Director 2", "Director 2"),
                    DateTime.UtcNow,
                    "English",
                    TimeSpan.FromHours(2),
                    true
                )
            };
            var command = new AddListOfMoviesCommand(listOfMovies);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(command, CancellationToken.None));
            await context.SaveChangesAsync();
            var count = await context.Medias.CountAsync();
            Assert.AreEqual(0, count);

        }
        [TestMethod]
        public async Task Handle_MultipleMoviesWithSameNewGenre_ShouldCreateOnlyOneGenre()
        {
            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                ("Movie 1",
                "Description 1",
                new GenreRequest("Genre 1"),
                new DirectorRequest("Director 1", "Director 1"),
                DateTime.UtcNow,
                "English",
                TimeSpan.FromHours(2),
                true
                ),
                new MovieRequest
                (
                    "Movie 2",
                    "Description 2",
                    new GenreRequest("Genre 1"),
                    new DirectorRequest("Director 1", "Director 1"),
                    DateTime.UtcNow,
                    "English",
                    TimeSpan.FromHours(2),
                    true
                )
            };
            var command = new AddListOfMoviesCommand(listOfMovies);

            await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();

            var genresInDb = await context.Genres.Where(g => g.Name.Value == "Genre 1").ToListAsync();
            Assert.AreEqual(1, genresInDb.Count, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
        }

    }
}
