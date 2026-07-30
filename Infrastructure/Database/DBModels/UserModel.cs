using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.DBModels
{
    public class UserModel : IdentityUser<Guid>
    {
        public UserModel() { }
        public UserModel(Guid? id, string username, string email)
        {
            Id = id ?? Guid.NewGuid();
            UserName = username;
            Email = email;
        }
        public UserModel(Guid? id, string username, string password, string email)
        {
            Id = id??Guid.NewGuid();
            UserName = username;
            PasswordHash = password;
            Email = email;
        }
    }
}
