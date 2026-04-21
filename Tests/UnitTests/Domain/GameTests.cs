using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;


namespace Tests.Domain
{
    
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Create_WithValidData_ShouldInitializeCorrectly()
        {
            var game = Game.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(),"Developer",new List<EPlatform>() { EPlatform.PC, EPlatform.XboxOne});
            Assert.IsNotNull(game);
            Assert.AreEqual("Title", game.Title);
            Assert.IsNotNull(game.Id);
            Assert.IsNotEmpty(game.Platforms);
            Assert.IsNotNull(game.Stats);
        }

        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsAdded()
        {
            var game = Game.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), "Developer", new List<EPlatform>() { EPlatform.PC, EPlatform.XboxOne });
            game.AddReview(Guid.NewGuid(), new Rating(4), "Good game!", new Username("Username"));
            Assert.AreEqual(4.0, game.Stats.AverageRating);
            game.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            game.DeleteReview(game.Reviews.First().Id);
            Assert.AreEqual(10.0, game.Stats.AverageRating);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenNewReviewIsDeleted()
        {
            var game = Game.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), "Developer", new List<EPlatform>() { EPlatform.PC, EPlatform.XboxOne });
            game.AddReview(Guid.NewGuid(), new Rating(4), "Good game!", new Username("Username"));
            game.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            Assert.AreEqual(7.0, game.Stats.AverageRating, 0.1);
            game.DeleteReview(game.Reviews.First().Id);
            Assert.AreEqual(10.0, game.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void UpdateRating_ShouldRecalculateAverage_WhenAllReviewIsDeleted()
        {
            var game = Game.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), "Developer", new List<EPlatform>() { EPlatform.PC, EPlatform.XboxOne });
            game.AddReview(Guid.NewGuid(), new Rating(4), "Good game!", new Username("Username"));
            game.AddReview(Guid.NewGuid(), new Rating(10), "Not bad", new Username("AnotherUser"));
            Assert.AreEqual(7.0, game.Stats.AverageRating, 0.1);
            game.DeleteReview(game.Reviews.First().Id);
            Assert.AreEqual(10.0, game.Stats.AverageRating, 0.1);
            game.DeleteReview(game.Reviews.First().Id);
            Assert.AreEqual(0.0, game.Stats.AverageRating, 0.1);
        }
        [TestMethod]
        public void AddReview_ShouldThrow_WhenUserTriesToReviewTwice()
        {
            var game = Game.Create("Title", "Desc", new Language("PL"), new ReleaseDate(DateTime.UtcNow), Guid.NewGuid(), "Dev", new List<EPlatform> { EPlatform.PC });
            var userId = Guid.NewGuid();
            game.AddReview(userId, new Rating(5), "First!", new Username("Player1"));

            Assert.Throws<DomainException>(() =>
                game.AddReview(userId, new Rating(1), "Second!", new Username("Player1")));
        }
        [TestMethod]
        public void SetReleaseDate_ShouldThrow_WhenDateIsInFuture()
        {
            Assert.Throws<DomainException>(()=>Game.Create("Title", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(2)), Guid.NewGuid(), "Developer", new List<EPlatform>() { EPlatform.PC, EPlatform.XboxOne }));

        }
    }
}
