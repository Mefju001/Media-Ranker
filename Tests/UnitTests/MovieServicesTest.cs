using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.MovieServices.AddListOfMovies;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.MovieServices.MovieUpsert;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace UnitTests
{
    [TestClass]
    public sealed class MovieServices
    {
        private GetMoviesByCriteriaHandler getMoviesByCriteriaHandler;
        private GetAllHandler getAllHandler;
        private MovieUpsertHandler movieUpsertHandler;
        private AddListOfMoviesHandler addListOfMoviesHandler;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMediator> _mediator;
        private Mock<IReferenceDataService> referenceMock;
        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            referenceMock = new Mock<IReferenceDataService>();
            _mediator = new Mock<IMediator>();
           // var sortAndFilter = new Mock<MovieSortAndFilterService>();
            movieUpsertHandler = new MovieUpsertHandler(_unitOfWork.Object, referenceMock.Object, _mediator.Object);
            getAllHandler = new GetAllHandler(_unitOfWork.Object);
            addListOfMoviesHandler = new AddListOfMoviesHandler(referenceMock.Object, _unitOfWork.Object);
            //getMoviesByCriteriaHandler = new GetMoviesByCriteriaHandler(_unitOfWork.Object, sortAndFilter.Object);
            var transacional = new Mock<IDbContextTransaction>();
            _unitOfWork.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(transacional.Object);
        }
        [TestMethod]
        public void GetAll()
        {
            List<MovieDomain> movies = new List<MovieDomain>
            {
                MovieDomain.Reconstruct(1,"Inception","A mind-bending thriller","polish",new DateTime(2010, 7, 16),1,1,TimeSpan.Zero,true),
                MovieDomain.Reconstruct(2,"Interstellar","A journey through space and time","english",new DateTime(2014, 11, 7),2,2,TimeSpan.Zero,false)

            };
            _unitOfWork.Setup(u => u.MovieRepository.GetAllAsync(new CancellationToken())).ReturnsAsync(movies);
            _unitOfWork.Setup(g => g.GenreRepository.Get(It.IsAny<int>())).Returns(Task.FromResult<GenreDomain>(GenreDomain.Reconstruct(1,"Sci-Fci")));
            var query = new GetAllQuery();
            var results = getAllHandler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Result.Count());
            _unitOfWork.Verify(u => u.MovieRepository.GetAllAsync(new CancellationToken()), Times.Once);
        }
        [TestMethod]
        public void UpsertMovie_AddMovie()
        {
            var command = new UpsertMovieCommand(
                null,
                "Inception",
                "A mind-bending thriller",
                new GenreRequest("Sci-Fci"),
                new DirectorRequest("Christopher", "Nolan"),
                new DateTime(2010, 7, 16),
                "English",
                TimeSpan.FromMinutes(148),
                true
            );
            referenceMock.Setup(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(GenreDomain.Create("Sci-Fci"));
            referenceMock.Setup(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>())).ReturnsAsync(DirectorDomain.Create("Christopher", "Nolan"));
            _unitOfWork.Setup(m => m.MovieRepository.AddAsync(It.IsAny<MovieDomain>())).ReturnsAsync((MovieDomain m) => m);
            var result = movieUpsertHandler.Handle(command, CancellationToken.None);
            referenceMock.Verify(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
            referenceMock.Verify(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>()), Times.Once);
            _unitOfWork.Verify(m => m.MovieRepository.AddAsync(It.IsAny<MovieDomain>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.Title, result.Result.Title);
        }
        [TestMethod]
        public void UpsertMovie_UpdateMovie()
        {
            var command = new UpsertMovieCommand(
                1,
                "Inception Updated",
                "A mind-bending thriller Updated",
                new GenreRequest("Sci-Fci"),
                new DirectorRequest("Christopher", "Nolan"),
                new DateTime(2010, 7, 16),
                "Polish",
                TimeSpan.FromMinutes(148),
                true
            );
            var existingMovie = MovieDomain.Reconstruct(1, "Inception", "A mind-bending thriller", "English", new DateTime(2010, 7, 16), 1, 1, TimeSpan.FromMinutes(148), true);
            referenceMock.Setup(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(GenreDomain.Create("Sci-Fci"));
            referenceMock.Setup(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>())).ReturnsAsync(DirectorDomain.Create("Christopher", "Nolan"));
            _unitOfWork.Setup(m => m.MovieRepository.FirstOrDefaultAsync(It.IsAny<int>())).ReturnsAsync(existingMovie);
            var result = movieUpsertHandler.Handle(command, CancellationToken.None);
            referenceMock.Verify(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Once);
            referenceMock.Verify(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>()), Times.Once);
            _unitOfWork.Verify(m => m.MovieRepository.FirstOrDefaultAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(command.Title, result.Result.Title);
            Assert.AreEqual(command.Description, result.Result.Description);
            Assert.AreEqual(command.Language, result.Result.Language);
        }
        [TestMethod]
        public void AddListOfMovies()
        {
            var command = new AddListOfMoviesCommand(new List<MovieRequest>
            {
                new MovieRequest(
                    "Inception",
                    "A mind-bending thriller",
                    new GenreRequest("Sci-Fci"),
                    new DirectorRequest("Christopher", "Nolan"),
                    new DateTime(2010, 7, 16),
                    "English",
                    TimeSpan.FromMinutes(148),
                    true
                ),
                new MovieRequest
                (
                    "Interstellar",
                    "A journey through space and time",
                    new GenreRequest("Sci-Fci"),
                    new DirectorRequest("Christopher", "Nolan"),
                    new DateTime(2014, 11, 7),
                    "English",
                    TimeSpan.FromMinutes(169),
                    false
                )
            });
            referenceMock.Setup(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>())).ReturnsAsync(GenreDomain.Create("Sci-Fci"));
            referenceMock.Setup(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>())).ReturnsAsync(DirectorDomain.Create("Christopher", "Nolan"));
            _unitOfWork.Setup(m => m.MovieRepository.AddAsync(It.IsAny<List<MovieDomain>>()));
            var results = addListOfMoviesHandler.Handle(command, CancellationToken.None);
            referenceMock.Verify(g => g.GetOrCreateGenreAsync(It.IsAny<GenreRequest>()), Times.Exactly(2));
            referenceMock.Verify(d => d.GetOrCreateDirectorAsync(It.IsAny<DirectorRequest>()), Times.Exactly(2));
            _unitOfWork.Verify(m => m.MovieRepository.AddAsync(It.IsAny<List<MovieDomain>>()), Times.Once);
            Assert.IsNotNull(results);
            Assert.HasCount(2, results.Result);
            Assert.AreEqual(command.movies[0].Title, results.Result[0].Title);
        }
        [TestMethod]
        public async Task GetMoviesByCriteria()
        {

            List<MovieDomain> movies = new List<MovieDomain>
            {
                MovieDomain.Reconstruct(1, "Inception", "A mind-bending thriller", "polish", new DateTime(2010, 7, 16), 1, 1, TimeSpan.Zero, true),
                MovieDomain.Reconstruct(2, "Interstellar", "A journey through space and time", "english", new DateTime(2014, 11, 7), 2, 2, TimeSpan.Zero, false),
                MovieDomain.Reconstruct(3, "The Dark Knight", "The Joker wreaks havoc on Gotham", "english", new DateTime(2008, 7, 18), 3, 1, TimeSpan.Zero, true),
                MovieDomain.Reconstruct(4, "The Matrix", "Reality is a simulation", "english", new DateTime(1999, 3, 31), 4, 3, TimeSpan.Zero, true)
            };
            movies.AsQueryable();
            _unitOfWork.Setup(u => u.MovieRepository.AsQueryable()).Returns(movies.AsQueryable());
            _unitOfWork.Setup(g=>g.GenreRepository.Get(It.IsAny<int>())).Returns(Task.FromResult<GenreDomain>(GenreDomain.Reconstruct(3,"Sci-Fci")));
            _unitOfWork.Setup(d=>d.DirectorRepository.Get(It.IsAny<int>())).Returns(Task.FromResult<DirectorDomain>(DirectorDomain.Reconstruct(1,"Christopher","Nolan")));
            //_unitOfWork.Setup(g => g.GenreRepository.FirstOrDefaultForNameAsync(It.IsAny<string>()));//.ReturnsAsync(GenreDomain.Create("Sci-Fci"));
            var query = new GetMoviesByCriteriaQuery();
            query.TitleSearch = "Inception";
            var results = await getMoviesByCriteriaHandler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(results);
            Assert.HasCount(1, results);
            Assert.AreEqual("Inception", results[0].Title);
            _unitOfWork.Verify(u => u.MovieRepository.AsQueryable(), Times.Once);
        }

    }
}
