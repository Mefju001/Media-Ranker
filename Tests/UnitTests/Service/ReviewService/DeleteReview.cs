using Application.Common.Interfaces;
using Application.Features.ReviewServices.DeleteReviewAsync;
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
    public class DeleteReview
    {
        private string username;
        private Guid mediaId;
        private Guid userId;
        private Guid reviewId;
        private Guid secondUserId;
        private AppDbContext appDbContext;
        private DeleteReviewHandler DeleteReviewHandler;
        private IMediaRepository<Media> mediaRepository;
        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            appDbContext = new AppDbContext(options);
            mediaRepository = new MediaRepository<Media>(appDbContext);
            DeleteReviewHandler = new DeleteReviewHandler(mediaRepository);
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
        public async Task DeleteReview_ShouldDeleteReview()
        {
            var command = new DeleteReviewCommand(mediaId, reviewId);
            var result = await DeleteReviewHandler.Handle(command, CancellationToken.None);
            var media = await mediaRepository.GetByIdAsync(mediaId, CancellationToken.None);
            Assert.IsTrue(result);
            Assert.IsFalse(media.Reviews.Any(r => r.Id == reviewId));
        }
        [TestMethod]
        public async Task DeleteReviewWhereMediaDoesNotExist_ShouldThrowNotFoundException()
        {
            var command = new DeleteReviewCommand(Guid.NewGuid(), reviewId);
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await DeleteReviewHandler.Handle(command, CancellationToken.None));
        }
        [TestMethod]
        public async Task DeleteReviewWhereReviewDoesNotExist_ShouldThrowDomainException()
        {
            var command = new DeleteReviewCommand(mediaId, Guid.NewGuid());
            await Assert.ThrowsExactlyAsync<DomainException>(async () => await DeleteReviewHandler.Handle(command, CancellationToken.None));
        }
    }
}
