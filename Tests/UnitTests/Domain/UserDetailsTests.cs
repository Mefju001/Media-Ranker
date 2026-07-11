using Domain.Aggregate;
using Domain.Enums;
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
        public void SetInteraction_ShouldAddNewInteraction()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var mediaId = Guid.NewGuid();
            userDetails.SetInteraction(mediaId, ETypeInteractions.WATCHING, ERatingVote.Liked);
            Assert.HasCount(1, userDetails.UserInteractions);
        }
        [TestMethod]
        public void RemoveInteraction_ShouldRemoveExistingInteraction()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var mediaId = Guid.NewGuid();
            userDetails.SetInteraction(mediaId, ETypeInteractions.WATCHING, null);
            userDetails.RemoveInteraction(mediaId);
            Assert.HasCount(0, userDetails.UserInteractions);

        }
        [TestMethod]
        public void SetInteraction_ShouldUpdateExistingInteraction()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            var mediaId = Guid.NewGuid();
            userDetails.SetInteraction(mediaId, ETypeInteractions.WATCHING, null);
            Assert.HasCount(1, userDetails.UserInteractions);
            Assert.AreEqual(ETypeInteractions.WATCHING, userDetails.UserInteractions.First().TypeInteractions);
            Assert.IsNull(userDetails.UserInteractions.First().RatingVote);
            userDetails.SetInteraction(mediaId, ETypeInteractions.COMPLETED, ERatingVote.Liked);
            Assert.HasCount(1, userDetails.UserInteractions);
            Assert.AreEqual(ETypeInteractions.COMPLETED, userDetails.UserInteractions.First().TypeInteractions);
            Assert.AreEqual(ERatingVote.Liked, userDetails.UserInteractions.First().RatingVote);
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
        public void UpdateProfile_ShouldThrow_WhenFullnameIsInvalid()
        {
            var userDetails = UserDetails.Create(Guid.NewGuid(), new Fullname("John", "Doe"), new Username("johndoe"), Email.Create("johndoe@example.com"));
            Assert.Throws<ArgumentException>(() => userDetails.UpdateProfile(new Fullname("", "")));
        }
    }
}
