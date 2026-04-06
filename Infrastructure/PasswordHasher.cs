using Domain.DomainService;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class PasswordHasher:IPasswordHasher
    {
        private readonly IPasswordHasher<UserModel> passwordHasher;
        public PasswordHasher(IPasswordHasher<UserModel> password)
        {
            passwordHasher = password;
        }
        public string CreatePasswordHash(string password)
        {
            var passwordHash = passwordHasher.HashPassword(null!, password);
            return passwordHash;
        }
    }
}
