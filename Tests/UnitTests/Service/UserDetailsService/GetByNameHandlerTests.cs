using Application.Behaviours;
using Application.Features.Auth.Common;
using Application.Features.Common.Interfaces;
using Application.Features.User.GetByName;
using Domain.Aggregate;
using Domain.Value_Object;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Service;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class GetByNameHandlerTests
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
            services.AddIdentity<UserModel, RoleModel>().AddEntityFrameworkStores<AppDbContext>();
            services.AddValidatorsFromAssembly(typeof(GetByNameQuery).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(GetByNameHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IIdentityService, IdentityService>();
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
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userModel = new UserModel(userId, "username", "password", "email");
            context.Users.Add(userModel);
            var user = UserDetails.Create(userId, new Fullname("Test", "User"), new Username("testuser"), Email.Create("testuser@example.com"));
            context.UsersDetails.Add(user);
            await context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Handle_GetUserByName_ShouldReturnUserResponse()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var user = await mediator.Send(new GetByNameQuery("Test"), CancellationToken.None);
            Assert.AreEqual(userId, user.id);
            Assert.AreEqual("Test", user.name);
            Assert.AreEqual("User", user.surname);
            Assert.IsNotNull(user);
        }
        [TestMethod]
        public async Task Handle_GetUserByName_ShouldReturnNull_WhenUserDoesNotExist()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var user = await mediator.Send(new GetByNameQuery("NonExistentUser"), CancellationToken.None);
            Assert.IsNull(user);
        }
    }
}
