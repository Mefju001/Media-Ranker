using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;
using Application.Features.Movies.GetByCriteria;
using Domain.Aggregate;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Service.MovieService
{
    [TestClass]
    public class GetByHandlerTests
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
            services.AddScoped<IMediaRepository<Media>, MediaRepository<Media>>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<ISortAndFilterService, SortAndFilterService>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            await SeedData();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.ChangeTracker.Clear();
            }
        }
        private async Task SeedData()
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var genre = Genre.Create("Action", Guid.NewGuid());
                db.Genres.Add(genre);
                var genre2 = Genre.Create("Adventure", Guid.NewGuid());
                db.Genres.Add(genre2);
                var director = Director.Create("Director1", "Director1", Guid.NewGuid());
                db.Directors.Add(director);
                var director2 = Director.Create("Director2", "Director2", Guid.NewGuid());
                db.Directors.Add(director2);
                var movie = Movie.Create("Title A", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-10)), genre.Id, director.Id, new Duration(TimeSpan.FromMinutes(120)), true);
                db.Medias.Add(movie);
                var movie2 = Movie.Create("Title B", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-15)), genre2.Id, director2.Id, new Duration(TimeSpan.FromMinutes(120)), true);
                db.Medias.Add(movie2);
                db.SaveChanges();
            }
            
        }


        [TestMethod]
        public async Task GetGamesByCriteria_WhenFilterByTitle_ShouldReturnMatch()
        {
            List<MovieResponse> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery("Title A", null, null, null, null, null, null, false);
                result = await mediator.Send(query, CancellationToken.None);

            }
            Assert.HasCount(1, result);
            Assert.AreEqual("Title A", result[0].Title);
        }

        [TestMethod]
        public async Task GetGamesByCriteria_WhenSortByDate_ShouldReturnOrdered()
        {
            List<MovieResponse> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery(null, null, null, null, null, null, "Date", true);
                result = await mediator.Send(query, CancellationToken.None);
            }
            Assert.HasCount(2, result);
            Assert.AreEqual("Title A", result[0].Title);
            Assert.AreEqual("Title B", result[1].Title);
        }
        [TestMethod]
        public async Task GetAllGamesAndDefaultSortShouldBeTitle()
        {
            List<MovieResponse> result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new GetByCriteriaQuery(null, null, null, null, null, null, null, false);
                result = await mediator.Send(query, CancellationToken.None);
            }
            Assert.HasCount(2, result);
            Assert.AreEqual("Title A", result[0].Title);
            Assert.AreEqual("Title B", result[1].Title);
        }
    }
}
