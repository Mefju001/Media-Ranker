using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.TvSeries.AddRange;
using Application.Features.TvSeries.Common;
using Domain.Aggregate;
using Domain.Enums;
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

namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class AddRangeHandlerTests
    {
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;

        private IMediaRepository<TvSeries> repository;
        private IGenreManager genreHelperService;
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
            services.AddScoped<IMediaRepository<TvSeries>, MediaRepository<TvSeries>>();
            services.AddScoped<IGenreManager, GenreManager>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
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
                    "Ongoing"
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
                    "Ongoing"
                )
            };
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new AddRangeCommand(listOfTvSeries);
            var result = await mediator.Send(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var moviesInDb = await context.Medias.ToListAsync();
            Assert.IsNotNull(moviesInDb);
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Title 1"));
            Assert.IsTrue(moviesInDb.Any(m => m.Title == "Title 2"));
        }
        [TestMethod]
        public async Task Handle_AddEmptyList_ShouldReturnEmptyList()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new AddRangeCommand(new List<TvSeriesRequest>());
            var result = await mediator.Send(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result);
        }
        [TestMethod]
        public async Task Handle_AddTvSeriesWithExistingGenre_ShouldCreateTvSeriesWithExistingGenre()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
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
                    "Ongoing"
                )
            };
            using var scope2 = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new AddRangeCommand(listOfTvSeries);
            var result = await mediator.Send(command, CancellationToken.None);
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
                    "Ongoing"
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
                    "Ongoing"
                )
            };
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new AddRangeCommand(listOfTvSeries);
            await Assert.ThrowsAsync<DomainException>(async () =>
                await mediator.Send(command, CancellationToken.None));
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
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
                    "Ongoing"
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
                    "Ongoing"
                )
            };
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new AddRangeCommand(listOfTvSeries);

            await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();

            var genresInDb = await context.Genres.Where(g => g.Name == "Name 1").ToListAsync();
            Assert.AreEqual(1, genresInDb.Count, "Gatunek o tej samej nazwie nie powinien zostać zduplikowany w bazie.");
        }

    }
}
