using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DBModels
{
    public class UserModel : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActived { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public UserModel() { }
        public UserModel(Guid id, string username, string password, string name, string surname, string email, DateTime createdAt, DateTime? UpdatedAt, bool isActived)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Name = name;
            Surname = surname;
            Email = email;
            CreatedAt = createdAt;
            this.UpdatedAt = UpdatedAt;
            IsActived = isActived;
        }
        public UserModel(Guid id, string username, string password, string name, string surname, string email, DateTime createdAt, DateTime? UpdatedAt, bool isActived, ICollection<IdentityUserRole<Guid>> Roles, ICollection<Token> Tokens)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Name = name;
            Surname = surname;
            Email = email;
            CreatedAt = createdAt;
            this.UpdatedAt = UpdatedAt;
            IsActived = isActived;
            this.Roles = Roles;
            this.Tokens = Tokens;
        }
    }
}
