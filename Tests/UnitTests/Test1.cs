using Application.Common.Interfaces;

using Application.Features.MovieServices.GetAll;
using Application.Mapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace UnitTests
{
    [TestClass]
    public sealed class MovieServices
    {
        private GetAllHandler _handler;
        private Mock<IUnitOfWork> _unitOfWork;
        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetAllHandler(_unitOfWork.Object);
        }
        [TestMethod]
        public void GetAll()
        {
            List<MovieDomain> movies = new List<MovieDomain>
            {
                MovieDomain.Reconstruct(1,"Inception","A mind-bending thriller","polish",new DateTime(2010, 7, 16),1,1,TimeSpan.Zero,true),
                MovieDomain.Reconstruct(2,"Interstellar","A journey through space and time","english",new DateTime(2014, 11, 7),2,2,TimeSpan.Zero,false)
                
            };
            _unitOfWork.Setup(u => u.MovieRepository.GetAllAsync()).ReturnsAsync(movies);
            var query = new GetAllQuery();
            var results = _handler.Handle(query, CancellationToken.None);
            Assert.IsNotNull(results);
            _unitOfWork.Verify(u => u.MovieRepository.GetAllAsync(), Times.Once);
        }
    }
}
