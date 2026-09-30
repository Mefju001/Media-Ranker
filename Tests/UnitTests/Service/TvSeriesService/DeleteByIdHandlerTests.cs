using Application.Behaviours;
using Application.Features.Common.Interfaces;
using Application.Features.Medias.TvSeries.DeleteById;
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

namespace Tests.Service.TvSeriesService
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
            services.AddScoped<IMediaRepository<TvSeries>, MediaRepository<TvSeries>>();
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
            var genreId = Guid.NewGuid();
            var directorId = Guid.NewGuid();
            var genre = Genre.Create("Genre1", genreId);
            appDbContext.Genres.Add(genre);
            var movieInDb = TvSeries.Create("Title 1","desc", "Lang", new ReleaseDate(DateTime.UtcNow),genre.Id, new SeasonDetails(2,30),"Netflix",ETvSeriesStatus.Canceled,id);
            appDbContext.Medias.Add(movieInDb);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_DeleteTvSeriesById_ShouldDeleteTvSeriesFromDb()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new DeleteByIdCommand(id), CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            var tvSeriesInDb = await appDbContext.Medias.FindAsync(id);
            Assert.IsNull(tvSeriesInDb, "TvSeries powinien zostać usunięty z bazy danych.");
        }
        [TestMethod]
        public async Task Handle_DeleteById_ShouldThrowNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var nonExistentMovieId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
            {
                await mediator.Send(new DeleteByIdCommand(nonExistentMovieId), CancellationToken.None);
            });
        }
    }
}
