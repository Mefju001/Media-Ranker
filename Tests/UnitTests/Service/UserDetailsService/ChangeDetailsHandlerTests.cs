using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Application.Features.Genres.GenreManager;
using Application.Features.User.ChangeDetails;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class ChangeDetailsHandlerTests
    {
        private Guid userId = Guid.NewGuid();
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
            services.AddValidatorsFromAssembly(typeof(ChangeDetailsCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(ChangeDetailsHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            services.AddLogging(builder => builder.AddConsole());
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
            }
            await SeedUser();
        }

        private async Task SeedUser()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var userModel = new UserModel(userId, "username", "password", "email");
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), "johndoe", Email.Create("johndoe@example.com"));
            context.Users.Add(userModel);
            context.UsersDetails.Add(user);
            await context.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_RequestWithEmptyGuid_ShouldThrowArgumentException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new ChangeDetailsCommand(Guid.Empty, "John", "Doe");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_RequestWithNonExistingUser_ShouldThrowUserNotFoundException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new ChangeDetailsCommand(Guid.NewGuid(), "John", "Doe");
            await Assert.ThrowsExactlyAsync<UserNotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ValidRequest_ShouldUpdateUserDetails()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new ChangeDetailsCommand(userId, "Jane", "Smith");
            await mediator.Send(command, CancellationToken.None);
            using var scope2 = _serviceProvider.CreateScope();
            var context = _serviceProvider.GetRequiredService<AppDbContext>();
            var updatedUser = await context.UsersDetails.FindAsync(userId);
            Assert.AreEqual("Jane", updatedUser.Fullname.FirstName);
            Assert.AreEqual("Smith", updatedUser.Fullname.LastName);
        }
        [TestMethod]
        public async Task Handle_WithEmptyRequest_ShouldThrowArgumentException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            var command = new ChangeDetailsCommand(userId, "", "");
            await Assert.ThrowsExactlyAsync<DomainException>(async () => await mediator.Send(command, CancellationToken.None));
        }
    }
}
