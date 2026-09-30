using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;

namespace Tests.Domain
{
    [TestClass]
    public class MovieTests
    {
        [TestMethod]
        public void Create_WithValidData_ShouldInitializeCorrectly()
        {
            var movie = Movie.Create("Title", "Description", "English", new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), Guid.NewGuid(), new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Cinema, EMovieStatus.Announced);
            Assert.IsNotNull(movie);
            Assert.AreEqual("Title", movie.Title);
            Assert.IsNotNull(movie.Id);
            Assert.IsNotNull(movie.Stats);
        }

        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsAdded()
        {
            var movie = Movie.Create("Title", "Description", "English", new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), Guid.NewGuid(), new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Cinema, EMovieStatus.Announced);
            movie.AddReview(Guid.NewGuid(), new Rating(4), "Good movie!", "Username");
            Assert.AreEqual(4.0, movie.Stats.AverageRating);
            movie.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", "AnotherUser");
            movie.DeleteReview(movie.Reviews.First().Id);
            Assert.AreEqual(10.0, movie.Stats.AverageRating);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsDeleted()
        {
            var movie = Movie.Create("Title", "Description", "English", new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), Guid.NewGuid(), new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Cinema, EMovieStatus.Announced);
            movie.AddReview(Guid.NewGuid(), new Rating(4), "Good movie!", "Username");
            movie.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", "AnotherUser");
            Assert.AreEqual(7.0, movie.Stats.AverageRating, 0.1);
            movie.DeleteReview(movie.Reviews.First().Id);
            Assert.AreEqual(10.0, movie.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenAllReviewIsDeleted()
        {
            var movie = Movie.Create("Title", "Description", "English", new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), Guid.NewGuid(), new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Cinema, EMovieStatus.Announced);
            movie.AddReview(Guid.NewGuid(), new Rating(4), "Good movie!", "Username");
            movie.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", "AnotherUser");
            Assert.AreEqual(7.0, movie.Stats.AverageRating, 0.1);
            movie.DeleteReview(movie.Reviews.First().Id);
            Assert.AreEqual(10.0, movie.Stats.AverageRating, 0.1);
            movie.DeleteReview(movie.Reviews.First().Id);
            Assert.AreEqual(0.0, movie.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void AddReview_ShouldThrow_WhenUserTriesToReviewTwice()
        {
            var movie = Movie.Create("Title", "Desc", "PL", new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), Guid.NewGuid(), new Duration(TimeSpan.FromMinutes(120)), EDistributionType.Cinema, EMovieStatus.Announced);
            var userId = Guid.NewGuid();
            movie.AddReview(userId, new Rating(5), "First!", "Player1");

            Assert.Throws<DomainException>(() =>
                movie.AddReview(userId, new Rating(1), "Second!", "Player1"));
        }
    }
}
