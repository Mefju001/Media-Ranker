
using Application.Features.ToWatchServices.Remove;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Tests.Service.ToWatchService
{
    [TestClass]
    public class Remove
    {
        private RemoveHandler handler;
        private IUserDetailsRepository userDetailsRepository;
        private AppDbContext appDbContext;
        private Guid userId = Guid.NewGuid();
        private Guid gameId = Guid.NewGuid();
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            appDbContext = new AppDbContext(options);
            userDetailsRepository = new UserDetailsRepository(appDbContext);
            handler = new RemoveHandler(userDetailsRepository);
            await SeedData();
        }
        private async Task SeedData()
        {
            var genreId = Guid.NewGuid();
            var genre = Genre.Create("Action", genreId);
            appDbContext.Genres.Add(genre);
            var game = Game.Create("Test Game", "Test Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), genreId, "developer", new List<EPlatform>() { EPlatform.PC }, gameId);
            appDbContext.Medias.Add(game);
            var user = UserDetails.Create(userId, new Fullname("Johnny", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            appDbContext.UsersDetails.Add(user);
            user.SetInteraction(gameId, null, null);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task TestRemove_ShouldRemoveMediaFromWatchList()
        {
            await handler.Handle(new RemoveCommand(gameId, userId), CancellationToken.None);
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(0, result.UserInteractions);
        }
        [TestMethod]
        public async Task TestRemove_ShouldThrowDomainException_WhenMediaIsNotInWatchList()
        {
            var nonExistentMediaId = Guid.NewGuid();
            await Assert.ThrowsExactlyAsync<DomainException>(async()=>await handler.Handle(new RemoveCommand(nonExistentMediaId, userId), CancellationToken.None));
            var result = appDbContext.UsersDetails.Include(u => u.UserInteractions).FirstOrDefault(u => u.Id == userId);
            Assert.IsNotNull(result);
            Assert.HasCount(1, result.UserInteractions);
        }
    }
}
