using Application.Common.Interfaces;
using Application.Features.UserServices.ChangeDetails;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class ChangeDetails
    {
        private AppDbContext context;
        private ChangeDetailsHandler handler;
        private IUserDetailsRepository userDetailsRepository;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            context = new AppDbContext(options);
            userDetailsRepository = new UserDetailsRepository(context);
            handler = new ChangeDetailsHandler(userDetailsRepository);
            await SeedUser();
        }
        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }
        private async Task SeedUser()
        {
            var user = UserDetails.Create(Guid.NewGuid(), new Fullname("Johnny", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            context.UsersDetails.Add(user);
            await context.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Handle_RequestWithEmptyGuid_ShouldThrowArgumentException()
        {
            var command = new ChangeDetailsCommand(Guid.Empty, "John", "Doe");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_RequestWithNonExistingUser_ShouldThrowUserNotFoundException()
        {
            var command = new ChangeDetailsCommand(Guid.NewGuid(), "John", "Doe");
            await Assert.ThrowsExactlyAsync<UserNotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task Handle_ValidRequest_ShouldUpdateUserDetails()
        {
            var existingUser = await context.UsersDetails.FirstAsync();
            var command = new ChangeDetailsCommand(existingUser.Id, "Jane", "Smith");
            await handler.Handle(command, CancellationToken.None);
            var updatedUser = await context.UsersDetails.FindAsync(existingUser.Id);
            Assert.AreEqual("Jane", updatedUser.Fullname.Name);
            Assert.AreEqual("Smith", updatedUser.Fullname.Surname);
        }
        [TestMethod]
        public async Task Handle_WithEmptyRequest_ShouldThrowArgumentException()
        {
            var existingUser = await context.UsersDetails.FirstAsync();
            var command = new ChangeDetailsCommand(existingUser.Id, "", "");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
