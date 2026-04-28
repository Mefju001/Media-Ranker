using Application.Common.Interfaces;
using Application.Features.UserServices.ChangePassword;
using Domain.Exceptions;
using MediatR;
using Moq;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class ChangePassword
    {
        private ChangePasswordHandler handler;
        private Mock<IIdentityService> identityService;

        [TestInitialize]
        public void Initialize()
        {
            identityService = new Mock<IIdentityService>();
            handler = new ChangePasswordHandler(identityService.Object);
        }
        [TestMethod]
        public async Task ChangePassword_ShouldChangePassword()
        {
            var command = new ChangePasswordCommand("newPassword", "newPassword", "hashedpassword", Guid.NewGuid());
            var result = await handler.Handle(command, CancellationToken.None);

            identityService.Verify(r => r.ChangePassword(
            command.userId,
            It.Is<string>(s => s == command.oldPassword),
            It.Is<string>(s => s == command.newPassword)),
            Times.Once);
            Assert.AreEqual(Unit.Value, result);
        }
        [TestMethod]
        public async Task ChangePassword_ShouldThrowPasswordMismatchException()
        {
            var command = new ChangePasswordCommand("newPassword", "differentNewPassword", "hashedpassword", Guid.NewGuid());
            await Assert.ThrowsExactlyAsync<PasswordMismatchException>(async () => await handler.Handle(command, CancellationToken.None));
            identityService.Verify(r => r.ChangePassword(
            command.userId,
            It.Is<string>(s => s == command.oldPassword),
            It.Is<string>(s => s == command.newPassword)),
            Times.Never);
        }
    }
}
