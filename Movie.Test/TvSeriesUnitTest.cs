using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Linq.Expressions;
using WebApplication1.Builder;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace MovieTest
{
    public class TvSeriesUnitTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IGenericRepository<TvSeries>> tvSeriesRepositoryMock;
        private readonly Mock<IGenericRepository<Genre>> genreRepositoryMock;
        private readonly Mock<IGenericRepository<Director>> directorRepositoryMock;
        private readonly Mock<IReferenceDataService> referenceDataServiceMock;
        private readonly Mock<ITvSeriesBuilder> tvSeriesBuilderMock;
        private readonly TvSeriesServices _sut;
        private readonly Mock<IMediator> mockMediator;
        public TvSeriesUnitTest()
        {
            tvSeriesRepositoryMock = new Mock<IGenericRepository<TvSeries>>();
            genreRepositoryMock = new Mock<IGenericRepository<Genre>>();
            directorRepositoryMock = new Mock<IGenericRepository<Director>>();
            referenceDataServiceMock = new Mock<IReferenceDataService>();
            tvSeriesBuilderMock = new Mock<ITvSeriesBuilder>();
            mockMediator = new Mock<IMediator>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            var transacional = new Mock<IDbContextTransaction>();
            unitOfWorkMock.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(transacional.Object);
            SetupUnitOfWork();
            _sut = new TvSeriesServices(
                mockMediator.Object,
                unitOfWorkMock.Object,
                tvSeriesBuilderMock.Object,
                referenceDataServiceMock.Object
                );
        }
        private void SetupUnitOfWork()
        {
            unitOfWorkMock.SetupGet(uow => uow.TvSeries).Returns(tvSeriesRepositoryMock.Object);
            unitOfWorkMock.SetupGet(uow => uow.Genres).Returns(genreRepositoryMock.Object);
        }
        private ITvSeriesBuilder SetupBuilderChain(TvSeries tvSeries)
        {
            var builderStepMock = new Mock<ITvSeriesBuilder>();
            builderStepMock.Setup(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>())).Returns(builderStepMock.Object);
            builderStepMock
                .Setup(b => b.WithGenre(It.IsAny<Genre>()))
                .Returns(builderStepMock.Object);
            builderStepMock
                .Setup(b => b.WithMetadata(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<EStatus>()))
                .Returns(builderStepMock.Object);

            builderStepMock.Setup(b => b.Build()).Returns(tvSeries);
            return builderStepMock.Object;
        }
        private TvSeriesRequest GetTvSeriesRequest()
        { 
            return new TvSeriesRequest(
                "Inception", "Great movie", new GenreRequest("Action"), DateTime.Now,
                "English", 1, 12, "Netflix", EStatus.Completed
            );
        }
        private TvSeries GetTvSeries()
        {
            return new TvSeries
            {
                Id = 1,
                title = "Inception",
                description = "Great movie",
                genre = new Genre { name = "Action" },
                ReleaseDate = DateTime.Now,
                Language = "English",
                Seasons = 1,
                Episodes = 12,
                Network = "Netflix",
                Status = EStatus.Completed,
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
        public async Task Upsert_AddSeries()
        {
            var testRequest = GetTvSeriesRequest();
            var createdGenre = new Genre { name = "Action", Id = 99 };
            referenceDataServiceMock.Setup(raf => raf.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(createdGenre);
            var tvSeries = GetTvSeries();
            var builderMock = SetupBuilderChain(tvSeries);
            tvSeriesBuilderMock.SetupSequence(b => b.CreateNew(It.IsAny<string>(), It.IsAny<string>())).Returns(builderMock);
            
            var result = await _sut.Upsert(null, testRequest);

            unitOfWorkMock.Verify(r => r.Genres.FirstOrDefaultAsync(It.IsAny<Expression<Func<Genre,bool>>>()), Times.Never);
            tvSeriesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TvSeries>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);

            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }
        [Fact]
        public async Task Upsert_UpdateSeries()
        {
            var testRequest = GetTvSeriesRequest();
            var createdGenre = new Genre { name = "Action", Id = 99 };
            var tvSeries = GetTvSeries();
            referenceDataServiceMock.Setup(raf => raf.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(createdGenre);
            tvSeriesRepositoryMock.Setup(t => t.FirstOrDefaultAsync(It.IsAny<Expression<Func<TvSeries, bool>>>())).ReturnsAsync(tvSeries);
            var result = await _sut.Upsert(tvSeries.Id, testRequest);
            Assert.NotNull(result);
            tvSeriesRepositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<TvSeries, bool>>>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            Assert.Equal("Inception", result.Title);
        }
        [Fact]
        public async Task GetAllAsync_ReturnsAllSeries()
        {
            List<TvSeries> movies = new List<TvSeries>()
            {
                GetTvSeries(),
                GetTvSeries()
            };
            tvSeriesRepositoryMock.Setup(t=>t.GetAllAsync()).ReturnsAsync(movies);
            var result = await _sut.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(movies.First().title, result.First().Title);
        }
        [Fact]
        public async Task GetAllAsync_ReturnsEmpySeries()
        {
            List<TvSeries> movies = new List<TvSeries>()
            {
            };
            tvSeriesRepositoryMock.Setup(t => t.GetAllAsync()).ReturnsAsync(movies);
            var result = await _sut.GetAllAsync();
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
