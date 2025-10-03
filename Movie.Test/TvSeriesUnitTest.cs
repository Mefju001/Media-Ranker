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
    public class TvSeriesUnitTest
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly MovieServices _movieServices;

        public TvSeriesUnitTest()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
        }
        [Fact]
        public async Task Upsert_AddMovie2()
        {
            var testRequest = new TvSeriesRequest(
                "Inception", "Great movie", new GenreRequest("Action"), DateTime.Now,
                "English", 1,12, "Netflix","Completed"
            );

            var GenRepoMock = new Mock<IGenericRepository<Genre>>();
            var TvSeriesRepoMock = new Mock<IGenericRepository<TvSeries>>();
            var transactionMock = new Mock<IDbContextTransaction>();

            GenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                       .ReturnsAsync(null as Genre);

            unitOfWorkMock.Setup(u => u.Genres).Returns(GenRepoMock.Object);
            unitOfWorkMock.Setup(u => u.TvSeries).Returns(TvSeriesRepoMock.Object);

            unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);
            unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            var builderMock = new Mock<ITvSeriesBuilder>();
            var tvSeries = new TvSeries
            {
                title = "Inception",
                description = "Great movie",
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Seasons = 1,
                Episodes = 12,
                Network = "Netflix",
                Status = "Completed"
            };
            builderMock.Setup(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithMetadata(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.WithGenre(It.IsAny<Genre>())).Returns(builderMock.Object);
            builderMock.Setup(b => b.Build()).Returns(tvSeries);
            var movieServicesMock = new TvSeriesServices(unitOfWorkMock.Object, builderMock.Object);

            var result = await movieServicesMock.Upsert(null, testRequest);

            GenRepoMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
            TvSeriesRepoMock.Verify(r => r.AddAsync(It.IsAny<TvSeries>()), Times.Once);

            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(result.response);
            Assert.Equal("Inception", result.response.Title);
        }
    }
}
