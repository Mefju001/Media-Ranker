using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Domain
{
    [TestClass]
    public class TvSeriesTests
    {
        public void Create_WithValidData_ShouldInitializeCorrectly()
        {
            var tvSeries = TvSeries.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), 2,30,"HBO",EStatus.EndedOrRemoved);
            Assert.IsNotNull(tvSeries);
            Assert.AreEqual("Title", tvSeries.Title);
            Assert.IsNotNull(tvSeries.Id);
            Assert.IsNotNull(tvSeries.Stats);
        }

        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsAdded()
        {
            var tvSeries = TvSeries.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), 2,30,"HBO",EStatus.EndedOrRemoved);
            tvSeries.AddReview(Guid.NewGuid(), new Rating(4), "Good series!", new Username("Username"));
            Assert.AreEqual(4.0, tvSeries.Stats.AverageRating);
            tvSeries.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            tvSeries.DeleteReview(tvSeries.Reviews.First().Id);
            Assert.AreEqual(10.0, tvSeries.Stats.AverageRating);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsDeleted()
        {
            var tvSeries = TvSeries.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), 2,30,"HBO",EStatus.EndedOrRemoved);
            tvSeries.AddReview(Guid.NewGuid(), new Rating(4), "Good series!", new Username("Username"));
            tvSeries.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            Assert.AreEqual(7.0, tvSeries.Stats.AverageRating, 0.1);
            tvSeries.DeleteReview(tvSeries.Reviews.First().Id);
            Assert.AreEqual(10.0, tvSeries.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenAllReviewIsDeleted()
        {
            var tvSeries = TvSeries.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), 2,30,"HBO",EStatus.EndedOrRemoved);
            tvSeries.AddReview(Guid.NewGuid(), new Rating(4), "Good series!", new Username("Username"));
            tvSeries.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            Assert.AreEqual(7.0, tvSeries.Stats.AverageRating, 0.1);
            tvSeries.DeleteReview(tvSeries.Reviews.First().Id);
            Assert.AreEqual(10.0, tvSeries.Stats.AverageRating, 0.1);
            tvSeries.DeleteReview(tvSeries.Reviews.First().Id);
            Assert.AreEqual(0.0, tvSeries.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void AddReview_ShouldThrow_WhenUserTriesToReviewTwice()
        {
            var tvSeries = TvSeries.Create("Title", "Desc", new Language("PL"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), 2,30,"HBO",EStatus.EndedOrRemoved);
            var userId = Guid.NewGuid();
            tvSeries.AddReview(userId, new Rating(5), "First!", new Username("Player1"));

            Assert.Throws<DomainException>(() =>
                tvSeries.AddReview(userId, new Rating(1), "Second!", new Username("Player1")));
        }
    }
}
