using Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Domain
{
    [TestClass]
    public class TokenTests
    {

        [TestMethod]
        public void Create_WithValidData_ShouldInitializeCorrectly()
        {
            var token = Token.Create("jti", "refreshToken", Guid.NewGuid(), null, null);
            Assert.IsNotNull(token);
            Assert.AreEqual("jti", token.Id);
        }
        [TestMethod]
        public void IsActiveAndBelongsToUser_ShouldReturnTrue_WhenTokenIsActiveAndBelongsToUser()
        {
            var userId = Guid.NewGuid();
            var token = Token.Create("jti", "refreshToken", userId, null, null);
            Assert.IsTrue(token.IsActiveAndBelongsToUser(userId));
        }
        [TestMethod]
        public void IsActiveAndBelongsToUser_ShouldReturnFalse_WhenTokenIsRevoked()
        {
            var userId = Guid.NewGuid();
            var token = Token.Create("jti", "refreshToken", userId, null, null);
            token.Revoke();
            Assert.IsFalse(token.IsActiveAndBelongsToUser(userId));
        }
        [TestMethod]
        public void Revoke_ShouldSetIsRevokedToTrueAndRevokedAt()
        {
            var token = Token.Create("jti", "refreshToken", Guid.NewGuid(), null, null);
            token.Revoke();
            Assert.IsTrue(token.IsRevoked);
            Assert.IsNotNull(token.RevokedAt);
        }
    }
}
