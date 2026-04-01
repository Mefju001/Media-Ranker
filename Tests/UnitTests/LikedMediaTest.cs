using Application.Common.Interfaces;
using Application.Features.LikedServices.AddLiked;
using Application.Features.LikedServices.Delete;
using Application.Features.LikedServices.GetAllLikedByUser;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Enums;
using Domain.Value_Object;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class LikedMediaTest
    {
        private AddLikedHandler handler;
        private DeleteLikedHandler DeleteLikedHandler;
        private GetAllLikedByUserHandler GetAllLikedByUserHandler;

        private Mock<> ;
        private Mock<ILikedMediaRepository> likedMediaRepository;
        private Mock<IGenreRepository> genreRepository;
        private Mock<IDirectorRepository> directorRepository;
        private Mock<IMediaRepository> mediaRepository;
        private Mock<IUserRepository> userRepository;

        private void SetupMocks()
        {
             = new Mock<>();
            likedMediaRepository = new Mock<ILikedMediaRepository>();
            genreRepository = new Mock<IGenreRepository>();
            directorRepository = new Mock<IDirectorRepository>();
            mediaRepository = new Mock<IMediaRepository>();
            userRepository = new Mock<IUserRepository>();
        }
        [TestInitialize]
        public void Setup()
        {
            SetupMocks();
            GetAllLikedByUserHandler = new GetAllLikedByUserHandler(mediaRepository.Object, userRepository.Object, genreRepository.Object, directorRepository.Object, likedMediaRepository.Object);
            DeleteLikedHandler = new DeleteLikedHandler(.Object, likedMediaRepository.Object, new Mock<ILogger<DeleteLikedHandler>>().Object);
            handler = new AddLikedHandler(new Mock<ILogger<AddLikedHandler>>().Object, .Object, likedMediaRepository.Object, directorRepository.Object, mediaRepository.Object, userRepository.Object, genreRepository.Object);
        }
        [TestMethod]
        public async Task AddLiked_ShouldAddLikedMedia()
        {
            // Arrange
            var command = new AddLikedCommand(Guid.NewGuid(), 1);
            var media = Game.Create("Test Game", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Test developer", EPlatform.PC);
            var user = User.Create(new Username("testuser"), new Password("password"), new Fullname("Test", "User"), new Email("test@wp.pl"));
            var userId = user.Id;
            var genre = Genre.Reconstruct(1, "Action");
            mediaRepository.Setup(m => m.GetMediaById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(media);
            userRepository.Setup(u => u.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            genreRepository.Setup(g => g.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(genre);
            likedMediaRepository.Setup(l => l.Any(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            // Act
            var result = handler.Handle(command, CancellationToken.None).Result;
            // Assert
            mediaRepository.Verify(m => m.GetMediaById(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            userRepository.Verify(u => u.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            genreRepository.Verify(g => g.Get(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            likedMediaRepository.Verify(l => l.Any(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.user.id, userId);
            Assert.AreEqual(result.Media.id, media.Id);
        }
        [TestMethod]
        public async Task DeleteLiked_ShouldDeleteLikedMedia()
        {
            // Arrange
            var command = new DeleteLikedCommand(Guid.NewGuid(), 1);
            var media = Game.Create("Test Game", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Test developer", EPlatform.PC);
            var user = User.Create(new Username("testuser"), new Password("password"), new Fullname("Test", "User"), new Email("test@"));
            likedMediaRepository.Setup(l => l.DeleteByLikedMedia(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
            var result = DeleteLikedHandler.Handle(command, CancellationToken.None);
            likedMediaRepository.Verify(l => l.DeleteByLikedMedia(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsTrue(result.Result);
        }
        [TestMethod]
        public async Task GetAllForUser_ShouldGetAllForUser()
        {
            var query = new GetAllLikedByUserQuery(Guid.NewGuid());
            var user = User.Create(new Username("testuser"), new Password("password"), new Fullname("Test", "User"), new Email("test@"));
            var media1 = Game.Create("Test Game 1", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Test developer", EPlatform.PC);
            var media2 = Game.Create("Test Game 2", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Test developer", EPlatform.PC);
            var media3 = Game.Create("Test Game 3", "Description", new Language("English"), new ReleaseDate(DateTime.UtcNow), 1, "Test developer", EPlatform.PC);
            var listOfLikedMedias = new List<LikedMedia>()
            {
                LikedMedia.Reconstruct(1, user.Id, 1),
                LikedMedia.Reconstruct(2, user.Id, 2),
                LikedMedia.Reconstruct(3, user.Id, 3)
            };
            likedMediaRepository.Setup(l => l.GetLikedForUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(listOfLikedMedias);
            userRepository.Setup(u => u.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            mediaRepository.Setup(m => m.GetByIds(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<int, Media>() { { 1, media1 }, { 2, media2 }, { 3, media3 } });
            genreRepository.Setup(g => g.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<int, Genre>() { { 1, Genre.Reconstruct(1, "Action") } });
            directorRepository.Setup(d => d.GetByIds(It.IsAny<List<int>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<int, Director>());
            var handler = await GetAllLikedByUserHandler.Handle(query, CancellationToken.None);
            Assert.HasCount(3, handler);
            Assert.AreEqual(1, handler[0].Media.Genre.id);

        }
    }
}
