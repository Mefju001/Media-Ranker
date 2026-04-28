using Application.Common.Interfaces;
using Application.Features.UserServices.DeleteUser;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class DeleteUserDetails
    {
        private Guid userId;
        private DeleteUserHandler handler;
        private Mock<IIdentityService> identityService;
        private AppDbContext appDbContext;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            identityService = new Mock<IIdentityService>();
            handler = new DeleteUserHandler(identityService.Object);
            await SeedData();
        }
        private async Task SeedData()
        {
            var user = new UserModel(Guid.NewGuid(), "testuser", "password", "testuser@example.com");
            userId = user.Id;
            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        [TestMethod]
        public async Task Handle_WithCorrectId_ShouldDeleteUser()
        {
            var command = new DeleteUserCommand(userId);
            var result = await handler.Handle(command, CancellationToken.None);
            identityService.Verify(x => x.DeleteUser(userId), Times.Once);
        }
        [TestMethod]
        public async Task Handle_WithWrongId_ShouldNeverDelete()
        {
            var wrongId = Guid.NewGuid();
            var command = new DeleteUserCommand(wrongId);
            var result = await handler.Handle(command, CancellationToken.None);
            identityService.Verify(x => x.DeleteUser(wrongId), Times.Never);
        }
        [TestMethod]
        public async Task Handle_WithEmptyId_ShouldNeverDelete()
        {
            var command = new DeleteUserCommand(Guid.Empty);
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
            identityService.Verify(x => x.DeleteUser(It.IsAny<Guid>()), Times.Never);
        }
    }
}
