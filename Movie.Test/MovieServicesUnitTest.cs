using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using SQLitePCL;
using System.IO;
using System.Linq.Expressions;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace MovieTest
{
    public class MovieServicesUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Movie>> movieRepositoryMock;
        private readonly Mock<IGenericRepository<Genre>> genreRepositoryMock;
        private readonly Mock<IGenericRepository<Director>> directorRepositoryMock;
        private readonly Mock<IReferenceDataService> _referenceDataServiceMock;
        private readonly Mock<IMovieBuilder> _movieBuilderMock;
        private readonly MovieServices _sut;
        private readonly Mock<IMediator> mockMediator;
        public MovieServicesUnitTest()
        {
            genreRepositoryMock = new Mock<IGenericRepository<Genre>>();
            directorRepositoryMock = new Mock<IGenericRepository<Director>>();
            movieRepositoryMock = new Mock<IGenericRepository<Movie>>();
            mockMediator = new Mock<IMediator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _referenceDataServiceMock = new Mock<IReferenceDataService>();
            _movieBuilderMock = new Mock<IMovieBuilder>();
            var transactionMock = new Mock<IDbContextTransaction>();
            _unitOfWorkMock.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
            SetupUnitOfWork();
            _sut = new MovieServices(
                _referenceDataServiceMock.Object,
                _unitOfWorkMock.Object,
                _movieBuilderMock.Object,
                mockMediator.Object
            );

        }
        private MovieRequest ReturnMovieRequest()
        {
            return new MovieRequest(
                    "Inception", "Great movie", new GenreRequest("Action"),
                    new DirectorRequest("John", "Doe"), DateTime.Now, "English",
                    new TimeSpan(2, 28, 0), true);
        }
        private Movie ReturnMovie()
        {
            var createdGenre = new Genre { name = "Action", Id = 99 };
            var createdDirector = new Director { name = "John", surname = "Doe", Id = 98 };
            return new Movie
            {
                title = "Inception",
                description = "Great movie",
                genre = createdGenre,
                director = createdDirector,
                ReleaseDate = DateTime.Now,
                Language = "English",
                Duration = new TimeSpan(2, 28, 0),
                IsCinemaRelease = true,
                Stats = new MediaStats
                {
                    Media = new Movie()
                    {
                        title = "Inception",
                        description = "Great movie",
                    }
                }
            };
        }
        private void SetupUnitOfWork()
        {
            _unitOfWorkMock.SetupGet(uow => uow.Movies).Returns(movieRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(uow => uow.Genres).Returns(genreRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(uow => uow.Directors).Returns(directorRepositoryMock.Object);
        }
        private void SetupMovieBuilderChain(Movie movie)
        {
            var createNewSequence = _movieBuilderMock.SetupSequence(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>()));
            var builderStepMock = new Mock<IMovieBuilder>();

            builderStepMock
                .Setup(b => b.WithTechnicalDetails(It.IsAny<TimeSpan>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<DateTime>()))
                .Returns(builderStepMock.Object);
            builderStepMock
                .Setup(b => b.WithGenre(It.IsAny<Genre>()))
                .Returns(builderStepMock.Object);
            builderStepMock
                .Setup(b => b.WithDirector(It.IsAny<Director>()))
                .Returns(builderStepMock.Object);

            var buildSetup = builderStepMock.SetupSequence(b => b.Build());
            createNewSequence = createNewSequence.Returns(builderStepMock.Object);
            buildSetup = buildSetup.Returns(movie);
        }
        private void SetupReferenceDataMocks(Genre genre, Director director)
        {
            _referenceDataServiceMock.Setup(raf => raf.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(genre);
            _referenceDataServiceMock.Setup(raf => raf.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>())).ReturnsAsync(director);

        }
        [Fact]
        public async Task Upsert_UpdateMovie()
        {
            var testRequest = ReturnMovieRequest();
            var stats = new MediaStats
            {
                Media = new Movie()
                {
                    title = "New Title",
                    description = "Some description",
                }
            };
            Movie movieInDb = new Movie()
            {
                Id = 1,
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
                Stats = stats
            };
            movieRepositoryMock.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>())).ReturnsAsync(movieInDb);
            var createdGenre = new Genre { name = "Action", Id = 99 };
            var createdDirector = new Director { name = "John", surname = "Doe", Id = 98 };
            SetupReferenceDataMocks(createdGenre,createdDirector);
            var movie = ReturnMovie();
            var result = await _sut.Upsert(1, testRequest);
            movieRepositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movie, bool>>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }
        [Fact]
        public async Task Upsert_AddMovie2()
        {
            var testRequest = ReturnMovieRequest();
            var createdGenre = new Genre { name = "Action", Id = 99 };
            var createdDirector = new Director { name = "John", surname = "Doe", Id = 98 };
            var stats = new MediaStats
            {
                Media = new Movie(){
                    title = "Inception",
                    description = "Great movie",
                }
            };
            var expectedMovie = new Movie
            {
                title = "Inception",
                description = "Great movie",
                genre = new Genre { name = "Action" },
                director = new Director { name = "John", surname = "Doe" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Duration = new TimeSpan(2, 28, 0),
                IsCinemaRelease = true,
                Stats = stats
            };

            SetupReferenceDataMocks(createdGenre,createdDirector);
            SetupMovieBuilderChain(expectedMovie);

            var result = await _sut.Upsert(null, testRequest);

            _unitOfWorkMock.Verify(u => u.Movies.AddAsync(It.Is<Movie>(m => m.title == "Inception")), Times.Once);

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync().Result.CommitAsync(), Times.Once);

            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMovies()
        {
            var stats = new MediaStats
            {
                Media = new Movie()
                {
                    title = "New Title",
                    description = "Some description",
                }
            };
            List<Movie> movies = new List<Movie>()
            {
             new Movie()
            {
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
                Stats = stats
            },
            new Movie()
            {
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
                Stats = stats
            }
            };
            movieRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(movies);
            var result = await _sut.GetAllAsync();

            movieRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(movies.First().title, result.First().Title);
        }
        
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieExists_ReturnsMovieResponse()
        {
            var stats = new MediaStats
            {
                Media = new Movie()
                {
                    title = "New Title",
                    description = "Some description",
                }
            };
            var movie = new Movie()
            {
                Id = 1,
                title = "New Title",
                description = "Some description",
                genre = new Genre { name = "New Genre" },
                director = new Director { name = "John", surname = "Doe" },
                Stats = stats
            };
            SetupUnitOfWork();
            movieRepositoryMock.Setup(u=>u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movie);
            var result = await _sut.GetById(movie.Id);
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("Some description", result.Description);
        }
        
        [Fact]
        public async Task GetMovieByIdAsync_WhenMovieDoesNotExist_ReturnsNull()
        {
            movieRepositoryMock.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<Movie,bool>>>())).ReturnsAsync((Movie?)null);
            SetupUnitOfWork();
            var result = await _sut.GetById(42);
            Assert.Null(result);
        }
    }
}
