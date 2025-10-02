using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;

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
            var testRequest = new MovieRequest(
                    "Inception", "Great movie", new GenreRequest("Action"),
                    new DirectorRequest("John", "Doe"), DateTime.Now, "English",
                    new TimeSpan(2, 28, 0), true);

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
                Platform = "...",
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
            };
            builderMock.Setup(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithTechnicalDetails(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithGenre(It.IsAny<Genre>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.Build()).Returns(game);
            var movieServicesMock = new GameServices(unitOfWorkMock.Object, builderMock.Object);

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
    }
}
