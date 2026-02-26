using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DBModels
{
    public class UserModel : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActived { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
        public UserModel() { }
        public UserModel(Guid id, string username, string password, string name, string surname, string email, DateTime createdAt, bool isActived)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Name = name;
            Surname = surname;
            Email = email;
            CreatedAt = createdAt;
            IsActived = isActived;
        }
        public UserModel(Guid id, string username, string password, string name, string surname, string email, DateTime createdAt, bool isActived, ICollection<IdentityUserRole<Guid>> Roles)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Name = name;
            Surname = surname;
            Email = email;
            CreatedAt = createdAt;
            IsActived = isActived;
            this.Roles = Roles;
        }
    }
}
