using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Medias.Movies.DeleteById;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
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
using Moq;

namespace Tests.Service.MovieService
{
    [TestClass]
    public class DeleteByIdHandlerTests
    {
        private readonly Guid id = Guid.NewGuid();
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
            services.AddValidatorsFromAssembly(typeof(DeleteByIdCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DeleteByIdHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IMediaRepository<Movie>, MediaRepository<Movie>>();
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
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var genreId = Guid.NewGuid();
            var directorId = Guid.NewGuid();
            var genre = Genre.Create("Genre1", genreId);
            db.Genres.Add(genre);
            var director = Director.Create("Director1", "Director1", directorId);
            db.Directors.Add(director);
            var movieInDb = Movie.Create("Test Movie", "Description", "English", new ReleaseDate(DateTime.UtcNow), genreId, directorId, new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Streaming, EMovieStatus.InProduction, id);
            db.Medias.Add(movieInDb);
            await db.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_DeleteMovieById_ShouldDeleteMovieFromDb()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var result = await mediator.Send(new DeleteByIdCommand(id), CancellationToken.None);
            }
            using (var assertScope = _serviceProvider.CreateScope())
            {
                var appDbContext = assertScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var movieInDb = await appDbContext.Medias.FindAsync(id);
                Assert.IsNull(movieInDb, "Film powinien zostać usunięty z bazy danych.");
            }
        }
        [TestMethod]
        public async Task Handle_DeleteById_ShouldThrowNotFoundException()
        {
            var nonExistentMovieId = Guid.NewGuid();
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<DeleteByIdCommand, Unit>>();
                await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                {
                    await handler.Handle(new DeleteByIdCommand(nonExistentMovieId), CancellationToken.None);
                });
            };
        }
    }
}
