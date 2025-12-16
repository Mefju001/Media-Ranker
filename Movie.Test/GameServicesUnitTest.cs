using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Linq.Expressions;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace MovieTest
{
    public class GameServicesUnitTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IGenericRepository<Game>> gameRepositoryMock;
        private readonly Mock<IGenericRepository<Genre>> genreRepositoryMock;
        private readonly Mock<IReferenceDataService> referenceDataServiceMock;
        private readonly Mock<IGameBuilder> gameBuilderMock;
        private readonly GameServices _sut;
        private readonly Mock<IMediator> mockMediator;

        public GameServicesUnitTest()
        {
            gameRepositoryMock = new Mock<IGenericRepository<Game>>();
            genreRepositoryMock = new Mock<IGenericRepository<Genre>>();
            referenceDataServiceMock = new Mock<IReferenceDataService>();
            gameBuilderMock = new Mock<IGameBuilder>();
            mockMediator = new Mock<IMediator>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            var transacional = new Mock<IDbContextTransaction>();
            unitOfWorkMock.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(transacional.Object);
            SetupUnitOfWork();
            _sut = new GameServices(
                unitOfWorkMock.Object,
                gameBuilderMock.Object,
                mockMediator.Object,
                referenceDataServiceMock.Object
                );

        }
        private void SetupUnitOfWork()
        {
            unitOfWorkMock.SetupGet(uow => uow.Games).Returns(gameRepositoryMock.Object);
            unitOfWorkMock.SetupGet(uow => uow.Genres).Returns(genreRepositoryMock.Object);
        
        }
        private IGameBuilder SetupIGameBuilder(Game game)
        {
            var gameBuilderMock = new Mock<IGameBuilder>();
            gameBuilderMock
                .Setup(b => b.WithTechnicalDetails(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(gameBuilderMock.Object);
            gameBuilderMock
                .Setup(b => b.WithGenre(It.IsAny<Genre>()))
                .Returns(gameBuilderMock.Object);
            gameBuilderMock
                .Setup(b => b.Build())
                .Returns(game);
            return gameBuilderMock.Object;
        }
        private GameRequest GetRequest()
        {
            return new GameRequest(
                "Inception", "Great movie", new GenreRequest("Action"), DateTime.Now,
                "English", null, EPlatform.Playstation
            );
        }
        private Game GetGame()
        {
            return new Game
            {
                title = "Inception",
                description = "Great movie",
                Platform = EPlatform.Unknown,
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Stats = new MediaStats()
                {
                    Media = new TvSeries()
                    {
                        title = "Inception",
                        description = "Great movie"
                    }
                }
            };
        }
        private Game GetGameInDb()
        {
            return new Game
            {
                Id = 1,
                title = "oldTitle",
                description = "oldDescription",
                Platform = EPlatform.Unknown,
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Stats = new MediaStats()
                {
                    Media = new TvSeries()
                    {
                        title = "Inception",
                        description = "Great movie"
                    }
                }
            };
        }
        [Fact]
        public async Task GetAll_ReturnsAllSeries()
        {
            List<Game> Games = new List<Game>()
            {
                GetGameInDb(),
                GetGameInDb()
            };
            gameRepositoryMock.Setup(g=>g.GetAllAsync()).ReturnsAsync(Games);
            var results = await _sut.GetAllAsync();
            Assert.Equal(2, results.Count);
        }
        [Fact]
        public async Task GetGameByIdAsync_WhenGameDoesNotExist()
        {
            const int id = 1;
            gameRepositoryMock.Setup(g => g.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync((Game) null);
            var result = await _sut.GetById(id);
            Assert.Null(result);
            unitOfWorkMock.Verify(u => u.Games.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task GetGameByIdAsync_WhenGameExists()
        {
            const int id = 1;
            gameRepositoryMock.Setup(g => g.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(GetGameInDb());
            var result = await _sut.GetById( id );
            Assert.NotNull(result);
            Assert.Equal("oldTitle", result.Title);
            Assert.Equal("oldDescription", result.Description);
            unitOfWorkMock.Verify(u => u.Games.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task Upsert_UpdateGame()
        {
            var notUpdatedGame = GetGameInDb();
            var testRequest = GetRequest();
            genreRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                       .ReturnsAsync(null as Genre);
            gameRepositoryMock.Setup(g=>g.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game,bool>>>())).ReturnsAsync(notUpdatedGame);
            var genre = new Genre { name = "Action", Id = 99 };
            referenceDataServiceMock.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(genre);
            var result = await _sut.Upsert(1, testRequest);
            gameRepositoryMock.Verify(g => g.FirstOrDefaultAsync(It.IsAny<Expression<Func<Game, bool>>>()),Times.Once);
            unitOfWorkMock.Verify(r => r.Genres.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Never);
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }
        [Fact]
        public async Task Upsert_AddMovie2()
        {
            var testRequest = GetRequest();
            genreRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                       .ReturnsAsync(null as Genre);

            var game = GetGame();
            var genre = new Genre { name="Action" ,Id = 99};
            var builderMock = SetupIGameBuilder(game);
            gameBuilderMock.Setup(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EPlatform>())).Returns(builderMock);
            referenceDataServiceMock.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(genre);

            var result = await _sut.Upsert(null, testRequest);

            gameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
            unitOfWorkMock.Verify(r => r.Genres.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Never);
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }
    }
}
