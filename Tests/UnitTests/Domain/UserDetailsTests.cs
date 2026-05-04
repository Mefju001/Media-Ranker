

using Domain.Aggregate;
using Domain.Value_Object;

namespace Tests.Domain
{
    [TestClass]
    public class UserDetailsTests
    {
        [TestMethod]
        public void Create_WithValidData_ShouldInitializeCorrectly()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            Assert.IsNotNull(userDetails);
            Assert.AreEqual("John", userDetails.Fullname.Name);
            Assert.AreEqual("Doe", userDetails.Fullname.Surname);
            Assert.IsTrue(userDetails.IsActive);
            Assert.IsNotNull(userDetails.AuditInfo);
        }
        [TestMethod]
        public void AddMediaToWatchList_ShouldAddMediaToList()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddToWatch(movieId);
            Assert.HasCount(1, userDetails.ToWatches);
            Assert.AreEqual(movieId, userDetails.ToWatches.First().MediaId);
        }
        [TestMethod]
        public void RemoveMediaFromWatchList_ShouldRemoveMediaFromList()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddToWatch(movieId);
            userDetails.RemoveToWatch(movieId);
            Assert.IsEmpty(userDetails.ToWatches);
        }
        [TestMethod]
        public void AddMediaToWatchList_ShouldThrow_WhenMediaIsAlreadyInWatchList()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddToWatch(movieId);
            Assert.Throws<InvalidOperationException>(() => userDetails.AddToWatch(movieId));
        }
        [TestMethod]
        public void AddLikedMedia_ShouldAddMediaToLikedList()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddLikedMedia(movieId);
            Assert.HasCount(1, userDetails.LikedMedias);
            Assert.AreEqual(movieId, userDetails.LikedMedias.First().MediaId);
        }
        [TestMethod]
        public void RemoveLikedMedia_ShouldRemoveMediaFromLikedList()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddLikedMedia(movieId);
            userDetails.RemoveLikedMedia(movieId);
            Assert.IsEmpty(userDetails.LikedMedias);
        }
        [TestMethod]
        public void UpdateProfile_ShouldUpdateFullnameAndAuditInfo()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var oldAuditInfo = userDetails.AuditInfo;

            userDetails.UpdateProfile(new Fullname("Jane", "Smith"));
            Assert.AreEqual("Jane", userDetails.Fullname.Name);
            Assert.AreEqual("Smith", userDetails.Fullname.Surname);
            Assert.IsNotNull(userDetails.AuditInfo.UpdatedAt);
        }
        [TestMethod]
        public void AddLikedMedia_ShouldThrow_WhenMediaIsAlreadyLiked()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var movieId = Guid.NewGuid();
            userDetails.AddLikedMedia(movieId);
            Assert.Throws<InvalidOperationException>(() => userDetails.AddLikedMedia(movieId));
        }
        [TestMethod]
        public void UpdateProfile_ShouldThrow_WhenFullnameIsInvalid()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            Assert.Throws<ArgumentException>(() => userDetails.UpdateProfile(new Fullname("", "")));
        }
    }
}
