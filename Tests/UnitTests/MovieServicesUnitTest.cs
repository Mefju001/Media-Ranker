using Application.Common.Interfaces;
using Application.Features.MovieServices.GetAll;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;


namespace MovieTest
{
    /*public class MovieServicesUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> mockMediator;
        private readonly GetAllHandler getAllHandler;
        public MovieServicesUnitTest()
        {
           getAllHandler = new GetAllHandler(_unitOfWorkMock.Object);
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            mockMediator = new Mock<IMediator>();
        }
        [Fact]
        public async Task GetAllMovies_ReturnsListOfMovies()
        {
            // Arrange
            var movies = new List<MovieDomain>
            {
                MovieDomain.Create("Movie 1", "Description 1","Polish",DateTime.Now,GenreDomain.Reconstruct(1,"Fantastyka"),DirectorDomain.Reconstruct(1,"name","surname"),TimeSpan.Zero),
            };
            _unitOfWorkMock.Setup(uow => uow.MovieRepository.GetAllAsync())
                .ReturnsAsync(movies);
            var query = new GetAllQuery();
            // Act
            var result = await getAllHandler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Movie 1", result[0].Title);
        }
    }*/
}
