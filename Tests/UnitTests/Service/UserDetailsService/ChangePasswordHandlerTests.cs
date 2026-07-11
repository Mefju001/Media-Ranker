using Application.Behaviours;
using Application.Features.Auth.Common;
using Application.Features.Common.Interfaces;
using Application.Features.User.ChangeDetails;
using Application.Features.User.ChangePassword;
using Domain.Exceptions;
using Domain.Repository;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using Infrastructure.Service;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class ChangePasswordHandlerTests
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

                services.AddValidatorsFromAssembly(typeof(ChangePasswordCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(ChangePasswordHandler).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<UserManager<UserModel>>();
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
            var oldPassword = "hashedpassword";
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using (var arrangeScope = _serviceProvider.CreateScope())
            {
                var userManager = arrangeScope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                var password = userManager.PasswordHasher.HashPassword(null, oldPassword);
                var user = new UserModel(userId, "username", password, "email@example.com");
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        [TestMethod]
        public async Task ChangePassword_ShouldChangePassword()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new ChangePasswordCommand("NewPassword2!", "NewPassword2!", "hashedpassword", userId);
            var result = await mediator.Send(command, CancellationToken.None);
            Assert.AreEqual(Unit.Value, result);
        }
        [TestMethod]
        public async Task ChangePassword_ShouldThrowPasswordMismatchException()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new ChangePasswordCommand("newPassword", "differentNewPassword", "hashedpassword", Guid.NewGuid());
            await Assert.ThrowsExactlyAsync<PasswordMismatchException>(async () => await mediator.Send(command, CancellationToken.None));
        }
    }
}
