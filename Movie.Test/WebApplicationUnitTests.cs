
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.Services;

namespace MovieTest
{
    public class WebApplicationUnitTests
    {
        [Fact]
        public async Task AddLikedMedia()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unikalna baza na test
                .Options;

            await using var context = new AppDbContext(options);

            context.Users.Add(new User { Id = 1, username = "TestUser", password = "abc", name = "test",surname="test",email = "test" });
            context.Movies.Add(new WebApplication1.Models.Movie { Id = 101, title = "TestMovie",description = "...",genre = new Genre { name="test"} , director = new Director { name = "test",surname = "test" },});
            await context.SaveChangesAsync();
            var service = new LikedMediaServices(context);

            var request = new LikedMediaRequest
            {
                MediaId = 101,
                UserId = 1
            };

            // Act
            var result = await service.Add(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result.user.username.ToString());
            Assert.Equal("TestMovie", result.Media.Title.ToString());
        }
    }
}