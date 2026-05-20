using Application.Behaviours;
using Application.Features.Auth.Common;
using Application.Features.Common.Interfaces;
using Application.Features.User.DeleteById;
using Domain.Exceptions;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Service;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class DeleteByIdHandlerTests
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
            services.AddValidatorsFromAssembly(typeof(DeleteByIdCommand).Assembly);
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DeleteByIdHandler).Assembly);
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
            await context.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_WithCorrectId_ShouldDeleteUser()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new DeleteByIdCommand(userId);
            var result = await mediator.Send(command, CancellationToken.None);
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = await appDbContext.Users.FirstOrDefaultAsync(u=>u.Id == userId);
            Assert.IsNull(user);
        }
        [TestMethod]
        public async Task Handle_WithWrongId_ShouldNeverDelete()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var wrongId = Guid.NewGuid();
            var command = new DeleteByIdCommand(wrongId);
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await mediator.Send(command, CancellationToken.None));
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Assert.IsNotNull(user);
        }
        [TestMethod]
        public async Task Handle_WithEmptyId_ShouldNeverDelete()
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new DeleteByIdCommand(Guid.Empty);
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await mediator.Send(command, CancellationToken.None));
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Assert.IsNotNull(user);
        }
    }
}
