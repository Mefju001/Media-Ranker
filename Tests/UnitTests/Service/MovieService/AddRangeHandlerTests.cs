using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Directors.Common;
using Application.Features.Directors.Manager;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.Movies.AddRange;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Service.MovieService
{
    [TestClass]
    public class AddRangeHandlerTests
    {
        private AppDbContext context;
        private AddRangeHandler handler;
        private IMediaRepository<Movie> movieRepository;
        private IGenreManager genreHelperService;
        private IDirectorManager directorHelperService;
        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            movieRepository = new MediaRepository<Movie>(context);
            genreHelperService = new GenreManager(new GenreRepository(context));
            directorHelperService = new DirectorManager(new DirectorRepository(context));
            handler = new AddRangeHandler(movieRepository, genreHelperService, directorHelperService);
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
            var command = new AddRangeCommand(listOfMovies);
            var result = await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            var moviesInDb = await context.Medias.ToListAsync();
            Assert.IsNotNull(moviesInDb);
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 1"));
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 2"));
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            var command = new AddRangeCommand(new List<MovieRequest>());
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
            var command = new AddRangeCommand(listOfMovies);
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
            var command = new AddRangeCommand(listOfMovies);
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
            var command = new AddRangeCommand(listOfMovies);

            await handler.Handle(command, CancellationToken.None);
            await context.SaveChangesAsync();

            var genresInDb = await context.Genres.Where(g => g.Name.Value == "Genre 1").ToListAsync();
            Assert.AreEqual(1, genresInDb.Count, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
        }

    }
}
