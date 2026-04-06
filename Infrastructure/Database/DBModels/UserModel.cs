using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.DBModels
{
    public class UserModel : IdentityUser<Guid>
    {
        public UserModel() { }
        public UserModel(Guid id, string username, string password, string email)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Email = email;
        }
    }
}
