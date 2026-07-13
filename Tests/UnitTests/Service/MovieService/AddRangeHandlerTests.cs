using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Directors.Common;
using Application.Features.Directors.Manager;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.Movies.AddRange;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Repository;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Service.MovieService
{
    [TestClass]
    public class AddRangeHandlerTests
    {
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        [TestInitialize]
        public async Task Initialize()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());
            services.AddValidatorsFromAssembly(typeof(AddRangeCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(AddRangeHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Movie>, MediaRepository<Movie>>();
            services.AddScoped<IGenreManager, GenreManager>();
            services.AddScoped<IDirectorManager, DirectorManager>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.ChangeTracker.Clear();
            }
        }
        [TestMethod]
        public async Task Handle_AddTwoMovies_ShouldCreateTwoMovies()
        {
            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                (
                    "Movie 1",
                    "Description 1",
                    new GenreRequest("Genre 1"),
                    new DirectorRequest("Director 1", "Director 1"),
                    DateTime.UtcNow,
                    "English",
                    TimeSpan.FromHours(2),
                    "Cinema",
                    "Released"
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
                    "Cinema",
                    "Released"
                )
            };
            List<Guid> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfMovies);
                result = await mediator.Send(command);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                Assert.IsNotNull(result);
                Assert.HasCount(2, result);
                var context = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var moviesInDb = await context.Medias.ToListAsync();
                Assert.IsNotNull(moviesInDb);
                Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 1"));
                Assert.IsTrue(moviesInDb.Any(m => m.Title == "Movie 2"));
            }
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            List<Guid> result;
            using (var actScope = _serviceProvider.CreateScope())
            {
                var mediator = actScope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(new List<MovieRequest>());
                result = await mediator.Send(command);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                Assert.IsNotNull(result);
                Assert.HasCount(0, result);
                var context = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var moviesInDb = await context.Medias.ToListAsync();
                Assert.IsNotNull(moviesInDb);
                Assert.IsEmpty(moviesInDb);
            }
        }
        [TestMethod]
        public async Task Handle_AddMovieWithExistingGenre_ShouldCreateMovieWithExistingGenre()
        {
            var existingGenre = Genre.Create("Existing Genre", Guid.NewGuid());

            using (var arrangeScope = _serviceProvider.CreateScope())
            {
                var arrangeContext = arrangeScope.ServiceProvider.GetRequiredService<AppDbContext>();
                arrangeContext.Genres.Add(existingGenre);
                await arrangeContext.SaveChangesAsync();
            }

            var listOfMovies = new List<MovieRequest>
            {
                new MovieRequest
                (
                    "Movie 1",
                    "Description 1",
                    new GenreRequest("Existing Genre"),
                    new DirectorRequest("Director 1", "Director 1"),
                    DateTime.UtcNow,
                    "English",
                    TimeSpan.FromHours(2),
                    "Cinema",
                    "Released"
                )
            };
            var command = new AddRangeCommand(listOfMovies);
            List<Guid> result;
            using (var actScope = _serviceProvider.CreateScope())
            {
                var mediator = actScope.ServiceProvider.GetRequiredService<IMediator>();
                result = await mediator.Send(command);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                var assertContext = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();

                Assert.IsNotNull(result);
                Assert.HasCount(1, result);
                var movieInDb = await assertContext.Medias.FirstOrDefaultAsync(m => m.Title == "Movie 1");
                Assert.IsNotNull(movieInDb);
                Assert.AreEqual(existingGenre.Id, movieInDb.GenreId);
            }
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
                "Cinema",
                "Released"
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
                    "Cinema",
                    "Released"
                )
            };
            using(var actScope = _serviceProvider.CreateScope())
            {
                var mediator = actScope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfMovies);
                await Assert.ThrowsAsync<DomainException>(async () =>
                    await mediator.Send(command));
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                var assertContext = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var moviesInDb = await assertContext.Medias.ToListAsync();
                Assert.IsNotNull(moviesInDb);
                Assert.IsEmpty(moviesInDb);
            }
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
                "Cinema",
                "Released"
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
                    "Cinema",
                    "Released"
                )
            };
            using (var actScope = _serviceProvider.CreateScope())
            {
                var mediator = actScope.ServiceProvider.GetRequiredService<IMediator>();
                var command = new AddRangeCommand(listOfMovies);
                await mediator.Send(command);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                var context = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var genresInDb = await context.Genres.Where(g => g.Name == "Genre 1").ToListAsync();
                Assert.HasCount(1, genresInDb, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
            }
        }
    }
}
