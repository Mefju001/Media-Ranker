using Application.Common.Interfaces;
using Application.Features.UserServices.ChangeDetails;
using Application.Features.UserServices.ChangePassword;
using Application.Features.UserServices.DeleteUser;
using Application.Features.UserServices.GetBy;
using Domain.Aggregate;
using Domain.Value_Object;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UserServicesTest
    {
        private GetUserByNameHandler getUserByNameHandler;
        private DeleteUserHandler deleteUserHandler;
        private ChangePasswordHandler changePasswordHandler;
        private ChangeDetailsHandler changeDetailsHandler;

        private Mock<IUserRepository> userRepository;
        private Mock<> ;

        private void SetupMocks()
        {
            userRepository = new Mock<IUserRepository>();
             = new Mock<>();
        }
        [TestInitialize]
        public void Initialize()
        {
            SetupMocks();
            getUserByNameHandler = new GetUserByNameHandler(userRepository.Object);
            deleteUserHandler = new DeleteUserHandler(userRepository.Object, Mock.Of<ILogger<DeleteUserHandler>>());
            changePasswordHandler = new ChangePasswordHandler(userRepository.Object, Mock.Of<ILogger<ChangePasswordHandler>>());
            changeDetailsHandler = new ChangeDetailsHandler(.Object, userRepository.Object);
        }
        [TestMethod]
        public async Task GetUserByNameHandler_ReturnsUserResponse_WhenUserExists()
        {
            var user = UserDetails.Create(new Username("test"), new Password("hashedpassword"), new Fullname("Test", "User"), new Email("test@test.pl"));

            userRepository.Setup(repo => repo.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(user);

            var result = await getUserByNameHandler.Handle(new GetUserByNameQuery("test"), CancellationToken.None);

            userRepository.Verify(repo => repo.GetUserByUsername("test"), Times.Once);
            Assert.AreEqual(user.Username.Value, result.username);
        }
        [TestMethod]
        public async Task DeleteUser_ShouldBeDeleted()
        {
            var user = UserDetails.Create(new Username("test"), new Password("hashedpassword"), new Fullname("Test", "User"), new Email("test@test.pl"));
            userRepository.Setup(repo => repo.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userRepository.Setup(repo => repo.DeleteUser(It.IsAny<UserDetails>())).ReturnsAsync(IdentityResult.Success);
            var result = await deleteUserHandler.Handle(new DeleteUserCommand(user.Id), CancellationToken.None);
            userRepository.Verify(repo => repo.GetUserById(user.Id, It.IsAny<CancellationToken>()), Times.Once);
            userRepository.Verify(repo => repo.DeleteUser(user), Times.Once);
        }
        [TestMethod]
        public async Task ChangePassword_ShouldChangePassword()
        {
            var user = UserDetails.Create(new Username("test"), new Password("hashedpassword"), new Fullname("Test", "User"), new Email("test@test.pl"));
            userRepository.Setup(r => r.ChangePassword(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(IdentityResult.Success);

            var command = new ChangePasswordCommand("newPassword", "hashedpassword", "newPassword", Guid.NewGuid());
            var result = await changePasswordHandler.Handle(command, CancellationToken.None);

            userRepository.Verify(r => r.ChangePassword(command.userId, command.oldPassword, command.newPassword), Times.Once);
            Assert.AreEqual(Unit.Value, result);
        }
        [TestMethod]
        public async Task ChangeDetails_ShouldChangeDetails()
        {
            var user = UserDetails.Create(new Username("test"), new Password("hashedpassword"), new Fullname("Test", "User"), new Email("test@"));
            var command = new ChangeDetailsCommand(user.Id, "newTest", "newUser", "newtest@");
            userRepository.Setup(r => r.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userRepository.Setup(r => r.IsAnyUserWhoHaveEmailAndId(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var result = changeDetailsHandler.Handle(command, CancellationToken.None);
            userRepository.Verify(r => r.GetUserById(command.userId, It.IsAny<CancellationToken>()), Times.Once);
            userRepository.Verify(r => r.IsAnyUserWhoHaveEmailAndId(command.email, command.userId), Times.Once);
            .Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(Unit.Value, result.Result);
        }
    }
}
