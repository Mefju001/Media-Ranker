using Application.Features.UserServices.GetBy;
using Domain.Aggregate;
using Domain.Value_Object;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Tests.Service.UserDetailsService
{
    [TestClass]
    public class GetByName
    {
        private Guid userID;
        private AppDbContext appDbContext;
        private GetUserByNameHandler handler;
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            handler = new GetUserByNameHandler(appDbContext);
            await SeedData();
        }
        private async Task SeedData()
        {
            var user = UserDetails.Create(userID = Guid.NewGuid(), new Fullname("Test", "User"), new Username("testuser"), Email.Create("testuser@example.com"));
            appDbContext.UsersDetails.Add(user);
            await appDbContext.SaveChangesAsync();
        }
        [TestCleanup]
        public void Cleanup()
        {
            appDbContext.Dispose();
        }
        [TestMethod]
        public async Task Handle_GetUserByName_ShouldReturnUserResponse()
        {
            var user = await handler.Handle(new GetUserByNameQuery("Test"), CancellationToken.None);
            Assert.AreEqual(userID, user.id);
            Assert.AreEqual("Test", user.name);
            Assert.AreEqual("User", user.surname);
            Assert.IsNotNull(user);
        }
        [TestMethod]
        public async Task Handle_GetUserByName_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var user = await handler.Handle(new GetUserByNameQuery("NonExistentUser"), CancellationToken.None);
            Assert.IsNull(user);
        }
    }
}
