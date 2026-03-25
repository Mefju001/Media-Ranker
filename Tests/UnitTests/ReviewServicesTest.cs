using Application.Common.Interfaces;
using Application.Features.ReviewServices.DeleteReviewAsync;
using Application.Features.ReviewServices.GetTheLastestReview;
using Application.Features.ReviewServices.UpsertReview;
using Domain.Entity;
using Domain.Value_Object;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class ReviewServicesTest
    {
        private ReviewUpsertHandler ReviewUpsertHandler;
        private GetTheLastestHandler GetTheLastestHandler;
        private DeleteReviewHandler DeleteReviewHandler;

        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IReviewRepository> reviewRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IMediaRepository> mediaRepositoryMock;

        private void InitializeMocks()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            reviewRepositoryMock = new Mock<IReviewRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            mediaRepositoryMock = new Mock<IMediaRepository>();
        }
        [TestInitialize]
        public void Setup()
        {
            InitializeMocks();
            DeleteReviewHandler = new DeleteReviewHandler(unitOfWorkMock.Object, reviewRepositoryMock.Object, Mock.Of<ILogger<DeleteReviewHandler>>());
            GetTheLastestHandler = new GetTheLastestHandler(reviewRepositoryMock.Object);
            ReviewUpsertHandler = new ReviewUpsertHandler(unitOfWorkMock.Object, Mock.Of<ILogger<ReviewUpsertHandler>>(), reviewRepositoryMock.Object, userRepositoryMock.Object, mediaRepositoryMock.Object);
        }
        [TestMethod]
        public async Task AddReviewToMedia_ShouldAddReview()
        {
            var user = User.Create(new Username("Username"), new Password("password"), new Fullname("name", "surname"), new Email("string@strin.pl"));
            var movie = Movie.Reconstruct(1, "Inception", "A mind-bending thriller", new Language("polish"), new ReleaseDate(new DateTime(2010, 7, 16)), 1, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6, 2));
            var command = new ReviewUpsertCommand(null, 1, user.Id, 5, "Great movie!");
            userRepositoryMock.Setup(x => x.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            mediaRepositoryMock.Setup(repo => repo.GetMediaById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
            var review = Review.Create(new Rating(command.Rating), command.Comment, movie.Id, user.Id, user.Username);
            reviewRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>())).ReturnsAsync(review);
            var result = await ReviewUpsertHandler.Handle(command, CancellationToken.None);
            userRepositoryMock.Verify(x => x.GetUserById(user.Id, It.IsAny<CancellationToken>()), Times.Once);
            mediaRepositoryMock.Verify(repo => repo.GetMediaById(1, It.IsAny<CancellationToken>()), Times.Once);
            reviewRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual(user.Username.Value, result.username);
        }
        [TestMethod]
        public async Task UpdateReview_ShouldUpdateReview()
        {
            var userId = Guid.NewGuid();
            var movie = Movie.Reconstruct(1, "Inception", "A mind-bending thriller", new Language("polish"), new ReleaseDate(new DateTime(2010, 7, 16)), 1, 1, new Duration(TimeSpan.Zero), true, new MediaStats(6, 2));
            var command = new ReviewUpsertCommand(1, 1, userId, 5, "Great movie!");
            reviewRepositoryMock.Setup(repo => repo.GetReviewByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(Review.Create(new Rating(3), "Bad movie", movie.Id, userId, new Username("Username")));
            var result = await ReviewUpsertHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual("Username", result.username);
        }
        [TestMethod]
        public async Task GetTheLastestReview_ShouldReturnTitle()
        {
            var titles = new List<string>()
            {
                "Inception",
                "The Dark Knight",
                "Interstellar"
            };
            reviewRepositoryMock.Setup(repo => repo.GetTheLastestReviewAsync(It.IsAny<CancellationToken>())).ReturnsAsync(titles);
            var result = await GetTheLastestHandler.Handle(new GetTheLastestQuery(), CancellationToken.None);
            reviewRepositoryMock.Verify(repo => repo.GetTheLastestReviewAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.HasCount(titles.Count, result);
        }
        [TestMethod]
        public async Task DeleteReview_ShouldDeleteReview()
        {
            var reviewId = 1;
            reviewRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(uow => uow.CompleteAsync(It.IsAny<CancellationToken>()));
            var result = await DeleteReviewHandler.Handle(new DeleteReviewCommand(reviewId), CancellationToken.None);
            reviewRepositoryMock.Verify(repo => repo.DeleteAsync(reviewId, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsTrue(result);

        }
    }
}
