using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        //private readonly MovieServices _movieServices;

        public MovieServicesUnitTest()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }
        /*[Fact]
        public async Task Upsert_UpdatesExistingMovie_WhenIdIsProvided()
        {
            var movieDbSetMock = new Mock<DbSet<WebApplication1.Models.Movie>>();
            var directorDbSetMock = new Mock<DbSet<Director>>();
            var genreDbSetMock = new Mock<DbSet<Genre>>();
            var director = new Director()
            {
                name = "TestName",
                surname = "TestName"
            };
            var genre = new Genre()
            {
                name = "TestName"
            };
            var movies = new List<WebApplication1.Models.Movie>
            {
                new WebApplication1.Models.Movie {
                    Id = 1,
                    title = "Inception",
                    description = "...",
                    director = director,
                    genre = genre
                }
            }.AsQueryable();
            // Zasymuluj zachowanie DbSet
            movieDbSetMock.As<IQueryable<WebApplication1.Models.Movie>>().Setup(m => m.Provider).Returns(movies.Provider);
            movieDbSetMock.As<IQueryable<WebApplication1.Models.Movie>>().Setup(m => m.Expression).Returns(movies.Expression);
            movieDbSetMock.As<IQueryable<WebApplication1.Models.Movie>>().Setup(m => m.ElementType).Returns(movies.ElementType);
            movieDbSetMock.As<IQueryable<WebApplication1.Models.Movie>>().Setup(m => m.GetEnumerator()).Returns(movies.GetEnumerator());

            // Zmockuj zachowanie FirstOrDefaultAsync, aby zwracał pojedynczy obiekt
            _dbContextMock.Setup(db => db.Movies).Returns(movieDbSetMock.Object);

            // Ustawienie mocka dla DbContext
            _dbContextMock.Setup(db => db.Movies).Returns(movieDbSetMock.Object);
            _dbContextMock.Setup(db => db.Directors).Returns(directorDbSetMock.Object);
            _dbContextMock.Setup(db => db.Genres).Returns(genreDbSetMock.Object);

            var movieRequest = new MovieRequest(
                Title: "New Title",
                Description: "Some description",
                Genre: new GenreRequest("New Genre"),
                Director: new DirectorRequest("John", "Doe"),
                ReleaseDate: DateTime.Now,
                Language: "English",
                Duration: new TimeSpan(1, 30, 0),
                IsCinemaRelease: true
            );
            // Act
           // var result = await _movieServices.Upsert(1, movieRequest);

            // Assert
           // Assert.Equal("New Title", result.response.Title);
            _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            // Assert.Equal(2, result.C);
        }*/
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
            var movieService = new MovieServices(_mapperMock.Object, _unitOfWorkMock.Object);

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
            var mockServices = new MovieServices(_mapperMock.Object,_unitOfWorkMock.Object);
            var result = await mockServices.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("Some description", result.Description);
        }
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieDoesNotExist_ReturnsNull()
        {
            _unitOfWorkMock.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Movie?)null);
            var mockServices = new MovieServices(_mapperMock.Object,_unitOfWorkMock.Object);
            var result = await mockServices.GetById(42);
            Assert.Null(result);
        }
    }
}
