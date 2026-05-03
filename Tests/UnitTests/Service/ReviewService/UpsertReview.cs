using Application.Common.Interfaces;
using Application.Features.ReviewServices.UpsertReview;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.ReviewService
{
    [TestClass]
    public class UpsertReview
    {
        private string username;
        private Guid mediaId;
        private Guid userId;
        private Guid reviewId;
        private Guid secondUserId;
        private AppDbContext appDbContext;
        private ReviewUpsertHandler ReviewUpsertHandler;
        private IMediaRepository<Media> mediaRepository;
        private IUserDetailsRepository userRepository;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            mediaRepository = new MediaRepository<Media>(appDbContext);
            userRepository = new UserDetailsRepository(appDbContext);
            ReviewUpsertHandler = new ReviewUpsertHandler(mediaRepository, userRepository);
            await SeedData();
        }
        private async Task SeedData()
        {
            var user = UserDetails.Create(Guid.NewGuid(), new Fullname("testuser", "testuser"), new Username("testuser"), Email.Create("testuser@example.com"));
            userId = user.Id;
            username = user.Username.Value;
            var genre = Genre.Create("Action", Guid.NewGuid());
            appDbContext.Genres.Add(genre);
            var genre2 = Genre.Create("Adventure", Guid.NewGuid());
            appDbContext.Genres.Add(genre2);
            var game = Game.Create("Game A", "Description A", new Language("English"), new ReleaseDate(DateTime.UtcNow.AddDays(-5)), genre2.Id, "Developer A", new List<EPlatform>() { EPlatform.PlayStation5 });
            mediaId = game.Id;
            game.AddReview(secondUserId, new Rating(4), "Good game!", new Username("testuser2"));
            reviewId = game.Reviews.First(r => r.UserId == secondUserId).Id;
            appDbContext.Medias.Add(game);
            appDbContext.UsersDetails.Add(user);
            await appDbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task AddReview_ShouldAddReview()
        {
            var command = new ReviewUpsertCommand(null, mediaId, userId, 5, "Great game!");
            var result = await ReviewUpsertHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual(username, result.username);
        }
        [TestMethod]
        public async Task UpdateReview_ShouldUpdateExistingReview()
        {
            var command = new ReviewUpsertCommand(reviewId, mediaId, secondUserId, 3, "Updated comment");
            var result = await ReviewUpsertHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.mediaId, result.MediaId);
            Assert.AreEqual(command.Rating, result.rating);
            Assert.AreEqual(command.Comment, result.comment);
            Assert.AreEqual("testuser2", result.username);
        }
        [TestMethod]
        public async Task UpsertReview_MissingUserId_ShouldThrowArgumentException()
        {
            var command = new ReviewUpsertCommand(null, mediaId, null, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await ReviewUpsertHandler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task UpsertReview_MissingMediaId_ShouldThrowArgumentException()
        {
            var command = new ReviewUpsertCommand(null, null, userId, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await ReviewUpsertHandler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task UpsertReview_NonExistentMedia_ShouldThrowNotFoundException()
        {
            var command = new ReviewUpsertCommand(null, Guid.NewGuid(), userId, 5, "Great game!");
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await ReviewUpsertHandler.Handle(command, CancellationToken.None));
        }
    }
}
