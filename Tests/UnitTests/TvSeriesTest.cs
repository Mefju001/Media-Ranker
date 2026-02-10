using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.TvSeriesServices.GetAll;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using Domain.Entity;
using Domain.Enums;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public sealed class TvSeriesTest
    {
        private Mock<IMediator> _mediator;
        private Mock<IReferenceDataService> _referenceDataService;
        private Mock<IUnitOfWork> _unitOfWork;
        private GetAllHandler _handler;
        private TvSeriesUpsertHandler _upsertHandler;

        [TestInitialize]
        public void Setup()
        {
            _referenceDataService = new Mock<IReferenceDataService>();
            _mediator = new Mock<IMediator>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetAllHandler(_unitOfWork.Object);
            _upsertHandler = new TvSeriesUpsertHandler(_unitOfWork.Object, _referenceDataService.Object, _mediator.Object);
        }
        [TestMethod]
        public async Task GetAllTvSeries_ReturnsTvSeriesList()
        {
            var TvSeriesList = new List<TvSeriesDomain>
            {
                TvSeriesDomain.Reconstruct(1, "Breaking Bad", "A high school chemistry teacher turned methamphetamine producer.", "English", new DateTime(2008, 1, 20), 1, 5, 62, "AMC", EStatus.Completed),
                TvSeriesDomain.Reconstruct(2, "Game of Thrones", "Nine noble families fight for control over the lands of Westeros.", "English", new DateTime(2011, 4, 17), 2, 8, 73, "HBO", EStatus.Completed)
            };
            _unitOfWork.Setup(u => u.TvSeriesRepository.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(TvSeriesList);
            var genresDictionary = new Dictionary<int, GenreDomain>
            {
                { 1, GenreDomain.Reconstruct(1, "Crime" )  },
                { 2, GenreDomain.Reconstruct(2, "Fantasy" ) }
            };
            _unitOfWork.Setup(u => u.GenreRepository.GetGenresDictionary()).Returns(Task.FromResult(genresDictionary));
            var result = await _handler.Handle(new GetAllTvSeriesQuery(), CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            _unitOfWork.Verify(u => u.TvSeriesRepository.GetAll(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.GenreRepository.GetGenresDictionary(), Times.Once);
        }
        [TestMethod]
        public async Task AddTvSeries()
        {
            var genre = GenreDomain.Reconstruct(1, "Crime");
            _referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).Returns(Task.FromResult(genre));
            var command = new UpsertTvSeriesCommand(null,
                "Breaking Bad",
                "A high school chemistry teacher turned methamphetamine producer.",
                new GenreRequest("Crime"),
                new DateTime(2008, 1, 20),
                "English",
                5,
                62,
                "AMC",
                EStatus.Completed
            );
            var createdTvSeries = TvSeriesDomain.Reconstruct(1, command.title, command.description, command.Language, command.ReleaseDate, genre.Id, command.Seasons, command.Episodes, command.Network, command.Status);
            _unitOfWork.Setup(u => u.TvSeriesRepository.AddTvSeriesAsync(It.IsAny<TvSeriesDomain>())).Returns(Task.FromResult(createdTvSeries));
            var result = await _upsertHandler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.title, result.Title);
            _unitOfWork.Verify(u => u.TvSeriesRepository.AddTvSeriesAsync(It.IsAny<TvSeriesDomain>()), Times.Once);
            _referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
        }
        [TestMethod]
        public async Task UpdateTvSeries()
        {
            var genre = GenreDomain.Reconstruct(1, "Crime");
            _referenceDataService.Setup(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).Returns(Task.FromResult(genre));
            var existingTvSeries = TvSeriesDomain.Reconstruct(1, "Breaking Bad", "A high school chemistry teacher turned methamphetamine producer.", "English", new DateTime(2008, 1, 20), genre.Id, 5, 62, "AMC", EStatus.Completed);
            _unitOfWork.Setup(u => u.TvSeriesRepository.GetTvSeriesById(It.IsAny<int>())).ReturnsAsync(existingTvSeries);
            var command = new UpsertTvSeriesCommand(1,
                "Breaking Bad Updated",
                "Updated description.",
                new GenreRequest("Crime"),
                new DateTime(2008, 1, 20),
                "English",
                5,
                62,
                "AMC",
                EStatus.Completed
            );
            var result = await _upsertHandler.Handle(command, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.title, result.Title);
            Assert.AreEqual(command.description, result.Description);
            _unitOfWork.Verify(u => u.TvSeriesRepository.GetTvSeriesById(It.IsAny<int>()), Times.Once);
            _referenceDataService.Verify(r => r.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
        }
    }
}
