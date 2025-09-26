using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;

namespace MovieTest
{
    public class MovieServicesUnitTest
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly MovieServices _movieServices;

        public MovieServicesUnitTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

        }
        private AppDbContext GetSqliteInMemoryContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }
        //do edycji
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
            var context = GetSqliteInMemoryContext();
            var unitOfWorkMock = new UnitOfWork(context);

            var movieService = new MovieServices(unitOfWorkMock);

            var result = await movieService.Upsert(null, testRequest);

            Assert.NotNull(result.response);
            Assert.Equal("Inception", result.response.Title);
        }
        [Fact]
        public async Task Upsert_AddMovie2()
        {
            var testRequest = new MovieRequest(
                    "Inception", "Great movie", new GenreRequest("Action"),
                    new DirectorRequest("John", "Doe"), DateTime.Now, "English",
                    new TimeSpan(2, 28, 0), true);

            var DirRepoMock = new Mock<IGenericRepository<Director>>();
            var GenRepoMock = new Mock<IGenericRepository<Genre>>();
            var MovieRepoMock = new Mock<IGenericRepository<Movie>>();
            var transactionMock = new Mock<IDbContextTransaction>();

            DirRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Director, bool>>>()))
                      .ReturnsAsync(null as Director);
            GenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                       .ReturnsAsync(null as Genre);

            unitOfWorkMock.Setup(u=>u.GenGenres).Returns(GenRepoMock.Object);
            unitOfWorkMock.Setup(u => u.GenMovies).Returns(MovieRepoMock.Object);
            unitOfWorkMock.Setup(u => u.GenDirectors).Returns(DirRepoMock.Object);

            unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
            unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var movieServicesMock = new MovieServices(unitOfWorkMock.Object);

            var result = await movieServicesMock.Upsert(null, testRequest);

            // 1. Sprawdzamy, czy wszystkie elementy zostały dodane do bazy (nowe Genre, Director i Movie)
            DirRepoMock.Verify(r => r.AddAsync(It.IsAny<Director>()), Times.Once);
            GenRepoMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
            MovieRepoMock.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Once);

            // 2. Sprawdzamy, czy zapisano zmiany i zatwierdzono transakcję
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            // 3. Weryfikacja wyniku (tylko dla pewności)
            Assert.NotNull(result.response);
            Assert.Equal("Inception", result.response.Title);
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsAllMovies()
        {
            List<Movie> movies = new List<Movie>()
            {
             new Movie()
            {
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
            },
            new Movie()
            {
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
            }
            };
            var MovieRepoMock = new Mock<IGenericRepository<Movie>>();


            MovieRepoMock.Setup(u => u.GetAllAsync()).ReturnsAsync(movies);
            unitOfWorkMock.Setup(u => u.Movies).Returns(MovieRepoMock.Object);
            var movieService = new MovieServices(unitOfWorkMock.Object);

            // Act
            var result = await movieService.GetAllAsync();

            MovieRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(movies.First().title, result.First().Title);
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
            };
            var MovieRepoMock = new Mock<IGenericRepository<Movie>>();
            MovieRepoMock.Setup(u=>u.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movie,bool>>>())).ReturnsAsync(movie);
            unitOfWorkMock.Setup(u => u.Movies).Returns(MovieRepoMock.Object);
            var mockServices = new MovieServices(unitOfWorkMock.Object);
            var result = await mockServices.GetById(movie.Id);
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("Some description", result.Description);
        }
        
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieDoesNotExist_ReturnsNull()
        {
            var MovieRepoMock = new Mock<IGenericRepository<Movie>>();
            MovieRepoMock.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movie,bool>>>())).ReturnsAsync((Movie?)null);
            unitOfWorkMock.Setup(u => u.Movies).Returns(MovieRepoMock.Object);
            var mockServices = new MovieServices(unitOfWorkMock.Object);
            var result = await mockServices.GetById(42);
            Assert.Null(result);
        }
    }
}
