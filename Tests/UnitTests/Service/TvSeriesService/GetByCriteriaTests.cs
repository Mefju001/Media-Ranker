using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.TvSeries.GetByCriteria;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Repository;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Service.TvSeriesService
{
    [TestClass]
    public class GetByCriteriaTests
    {
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
            services.AddValidatorsFromAssembly(typeof(GetByCriteriaQuery).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(GetByCriteriaHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<TvSeries>, MediaRepository<TvSeries>>();
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
            var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var tvSeries = TvSeries.Create("Title 1","desc","Lang", new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, new SeasonDetails(2,30),"Netflix",ETvSeriesStatus.Canceled);
            appDbContext.Medias.Add(tvSeries);
            var tvSeries2 = TvSeries.Create("Title 2", "desc", "Lang", new ReleaseDate(DateTime.UtcNow.AddDays(-15)), genre2.Id, new SeasonDetails(2,30),"Netflix",ETvSeriesStatus.Canceled);
            appDbContext.Medias.Add(tvSeries2);
            appDbContext.SaveChanges();
        }


        [TestMethod]
        public async Task GetTvSeriesByCriteria_WhenFilterByTitle_ShouldReturnMatch()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var query = new GetByCriteriaQuery
            {
                TitleSearch = "Title 1"
            };
            var result = await mediator.Send(query, CancellationToken.None);
            Assert.HasCount(1, result);
            Assert.AreEqual("Title 1", result[0].Title);
        }

        [TestMethod]
        public async Task GetTvSeriesByCriteria_WhenSortByDate_ShouldReturnOrdered()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var query = new GetByCriteriaQuery
            {
                SortByField = "Date",
                IsDescending = true
            };
            var result = await mediator.Send(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title 1", result[0].Title);
            Assert.AreEqual("Title 2", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllTvSeriesAndDefaultSortShouldBeTitle()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var query = new GetByCriteriaQuery
            {
            };
            var result = await mediator.Send(query, CancellationToken.None);
            Assert.HasCount(2, result);
            Assert.AreEqual("Title 1", result[0].Title);
            Assert.AreEqual("Title 2", result[1].Title);
        }
    }
}
