using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using Microsoft.AspNetCore.Identity;


namespace Domain.DomainServices
{
    public class UserPasswordService : IUserPasswordService
    {
        private readonly IPasswordHasher<User> Hasher;
        public UserPasswordService(IPasswordHasher<User> hasher)
        {
            Hasher = hasher;
        }
        public Password GenerateNewPassword(User user, string oldPassword, string newPassword)
        {
            var verification = Hasher.VerifyHashedPassword(user, user.Password.HashValue, oldPassword);
            if (verification == PasswordVerificationResult.Failed)
                throw new InvalidCredentialsException("You write wrong Password");

            if (oldPassword == newPassword)
                throw new NewPasswordIsSameAsOldException("The new password is too similar to the old one");

            var newHash = Hasher.HashPassword(user, newPassword);
            return new Password(newHash);
        }
    }
}
