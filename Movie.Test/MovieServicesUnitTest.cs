using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services.Impl;

namespace MovieTest
{
    public class MovieServicesUnitTest
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly MovieServices _movieServices;

        public MovieServicesUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

        }
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unikalna baza na test
                .Options;
                var context = new AppDbContext(options);
                return context;
        }
        [Fact]
        public async Task Upsert_AddMovie()
        {
            var testRequest = new MovieRequest(
                    "Inception",
                    "Great movie",
                    new GenreRequest("Action"),
                    new DirectorRequest("John", "Doe"),
                    DateTime.Now,
                    "English",
                    new TimeSpan(2, 28, 0),
                    true
                );
            var context = GetInMemoryDbContext();
            var unitOfWorkMock = new Mock<UnitOfWork>(context);

            var movieService = new MovieServices(unitOfWorkMock.Object);

            var result = await movieService.Upsert(null, testRequest);

            Assert.NotNull(result.response);
            Assert.Equal("Inception", result.response.Title);
        }
        [Fact]
        public async Task AddMovie()
        {
            var testRequest = new MovieRequest(
                    "Inception",
                    "Great movie",
                    new GenreRequest("Action"),
                    new DirectorRequest("John", "Doe"),
                    DateTime.Now,
                    "English",
                    new TimeSpan(2, 28, 0),
                    true
                );
            var context = GetInMemoryDbContext();
            var unitOfWorkMock = new Mock<UnitOfWork>(context);
            unitOfWorkMock.Setup(u => u.Directors.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Director, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Director)null);

            unitOfWorkMock.Setup(u => u.Genres.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Genre)null);

            
            unitOfWorkMock.Setup(u => u.Movies.Add(It.IsAny<Movie>()));
            unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            
            var movieService = new MovieServices(unitOfWorkMock.Object);

            var result = await movieService.Upsert(null, testRequest);

            Assert.NotNull(result.response);
            Assert.Equal("Inception", result.response.Title);
            Assert.Equal("Great movie", result.response.Description);

            // Sprawdzamy, czy Add i CompleteAsync były wywołane
            unitOfWorkMock.Verify(u => u.Movies.Add(It.IsAny<Movie>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAllAsync_ReturnsAllMovies()
        {
            List<Movie> movies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    title = "New Title",
                    description = "Some description",
                    genre = new Genre { name = "New Genre" },
                    director = new Director { name = "John", surname = "Doe" },
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = new TimeSpan(1, 30, 0),
                    IsCinemaRelease = true
                },
                new Movie()
                {
                    Id = 2,
                    title = "New Title",
                    description = "Some description",
                    genre = new Genre { name = "New Genre" },
                    director = new Director { name = "John", surname = "Doe" },
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = new TimeSpan(1, 30, 0),
                    IsCinemaRelease = true
                },
                new Movie()
                {
                    Id = 3,
                    title = "New Title",
                    description = "Some description",
                    genre = new Genre { name = "New Genre" },
                    director = new Director { name = "John", surname = "Doe" },
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = new TimeSpan(1, 30, 0),
                    IsCinemaRelease = true
                }
            };

            _unitOfWorkMock.Setup(u => u.GetAllAsync()).ReturnsAsync(movies);
            var movieService = new MovieServices(_unitOfWorkMock.Object);

            // Act
            var result = await movieService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieExists_ReturnsMovieResponse()
        {
            var movie = new Movie()
            {
                Id = 1,
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Duration = new TimeSpan(1, 30, 0),
                IsCinemaRelease = true
            };
            _unitOfWorkMock.Setup(u=>u.GetByIdAsync(1)).ReturnsAsync(movie);
            var mockServices = new MovieServices(_unitOfWorkMock.Object);
            var result = await mockServices.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("Some description", result.Description);
        }
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieDoesNotExist_ReturnsNull()
        {
            _unitOfWorkMock.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Movie?)null);
            var mockServices = new MovieServices(_unitOfWorkMock.Object);
            var result = await mockServices.GetById(42);
            Assert.Null(result);
        }
    }
}
