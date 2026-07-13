using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Genres.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.TvSeries.Upsert;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;
using Moq;


namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class UpsertHandlerTests
    {
        private Guid tvSeriesId;
        private SqliteConnection _connection;
        private IServiceProvider _serviceProvider;
        [TestInitialize]
        public async Task Setup()
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
            services.AddValidatorsFromAssembly(typeof(UpsertCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(UpsertHandler).Assembly);
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
            await SeedData();
        }
        private async Task SeedData()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var genre = Genre.Create("Action", Guid.NewGuid());
            context.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            context.Genres.Add(genre2);
            var tvSeries = TvSeries.Create("Title", "desc", "Lang", new ReleaseDate(DateTime.UtcNow), genre.Id, new SeasonDetails(2,30), "Netflix", ETvSeriesStatus.Ongoing);
            context.Medias.Add(tvSeries);
            tvSeriesId = tvSeries.Id;
            context.SaveChanges();
        }
        [TestMethod]
        public async Task Handle_WhenIdIsNull_ShouldCreateNewTvSeries()
        {
            var command = new UpsertCommand(
                null,
                "New Title",
                "desc",
                new GenreRequest("name"),
                new ReleaseDate(DateTime.UtcNow),
                "Lang",
                2,
                20,
                "Netflix",
                "Ongoing"
                );

            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var tvSeriesInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Title");
            Assert.IsNotNull(tvSeriesInDb);
            Assert.AreEqual("New Title", tvSeriesInDb.Title);
        }

        [TestMethod]
        public async Task Handle_WhenIdIsNotNull_ShouldUpdateExistingTvSeries()
        {
            var command = new UpsertCommand(
                            tvSeriesId,
                            "New Title",
                            "Description",
                            new GenreRequest("Adventure"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            "Ongoing"
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var gameInDb = await context.Medias.FirstOrDefaultAsync(g => g.Title == "New Title");
            Assert.IsNotNull(gameInDb);
            Assert.AreEqual("New Title", gameInDb.Title);
            Assert.AreEqual("Description", gameInDb.Description);
           
        }
        [TestMethod]
        public async Task Handle_WhenGenreDoesNotExist_ShouldCreateNewGenre()
        {
            var command = new UpsertCommand(
                null,
                "New Title",
                "Description",
                new GenreRequest("New Genre"),
                new ReleaseDate(DateTime.UtcNow),
                "Lang",
                2,
                20,
                "Netflix",
                "Ongoing"
                );
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Name == "New Genre");
            Assert.IsNotNull(genreInDb);
            Assert.AreEqual("New Genre", genreInDb.Name);
        }
        [TestMethod]
        public async Task Handle_GenreRequestIsEmpty_ShouldThrowArgumentException()
        {
            var command = new UpsertCommand(
                            null,
                            "New Title",
                            "Description",
                            new GenreRequest(string.Empty),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            "Ongoing"
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_WhenTvSeriesDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new UpsertCommand(
                            Guid.NewGuid(),
                            "New Title",
                            "Description",
                            new GenreRequest("Action"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            "Ongoing"
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            await Assert.ThrowsAsync<NotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ChangeGenreToExisting_ShouldUpdateTvSeriesGenre()
        {
            var command = new UpsertCommand(
                            tvSeriesId,
                            "New Title",
                            "Description",
                            new GenreRequest("Adventure"),
                            new ReleaseDate(DateTime.UtcNow),
                            "Lang",
                            2,
                            20,
                            "Netflix",
                            "Ongoing"
                            );
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var tvSeriesInDb = await context.Medias.FirstOrDefaultAsync(g => g.Id == tvSeriesId);
            var genreInDb = await context.Genres.FirstOrDefaultAsync(g => g.Id == tvSeriesInDb.GenreId);
            Assert.IsNotNull(tvSeriesInDb);
            Assert.AreEqual("Adventure", genreInDb.Name);
        }
    }
}
