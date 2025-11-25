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
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly MovieServices _movieServices;

        public GameServicesUnitTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

        }
        [Fact]
        public async Task Upsert_AddMovie2()
        {
            var testRequest = new GameRequest(
                "Inception","Great movie",new GenreRequest( "Action" ),DateTime.Now,
                "English",null, EPlatform.Playstation
            );

            var GenRepoMock = new Mock<IGenericRepository<Genre>>();
            var GameRepoMock = new Mock<IGenericRepository<Game>>();
            var transactionMock = new Mock<IDbContextTransaction>();

            GenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                       .ReturnsAsync(null as Genre);

            unitOfWorkMock.Setup(u => u.Genres).Returns(GenRepoMock.Object);
            unitOfWorkMock.Setup(u => u.Games).Returns(GameRepoMock.Object);

            unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
            unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            var builderMock = new Mock<IGameBuilder>();
            var game = new Game
            {
                title = "Inception",
                description = "Great movie",
                Platform = EPlatform.Unknown,
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
            };
            var genre = new Genre { name="Action" ,Id = 99};
            builderMock.Setup(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EPlatform>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithTechnicalDetails(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithGenre(It.IsAny<Genre>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.Build()).Returns(game);
            var mediatorMock = new Mock<IMediator>();
            var IRefMock = new Mock<IReferenceDataService>();
            IRefMock.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(genre);
            var movieServicesMock = new GameServices(unitOfWorkMock.Object, builderMock.Object, mediatorMock.Object, IRefMock.Object);

            var result = await movieServicesMock.Upsert(null, testRequest);

            // 1. Sprawdzamy, czy wszystkie elementy zostały dodane do bazy (nowe Genre, Director i Movie)
          //  GenRepoMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
            GameRepoMock.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);

            // 2. Sprawdzamy, czy zapisano zmiany i zatwierdzono transakcję
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            // 3. Weryfikacja wyniku (tylko dla pewności)
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }
    }
}
