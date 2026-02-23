using Domain.DomainServices;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace Domain.Entity
{
    public class User
    {
        public Guid Id { get; private set; }
        public Username username { get; private set; }
        public Password password { get; private set; }
        public Fullname Fullname { get; private set; }
        public CreatedAt CreatedAt { get; private set; }
        public bool IsActived { get; private set; }
        public Email Email { get; private set; }
        private readonly List<Role> Roles = new();
        public virtual IReadOnlyCollection<Role> UserRoles => Roles.AsReadOnly();

        private User() { }
        private User(Guid id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool isActived)
        {
            Id = id;
            this.username = username;
            password = passwordHash;
            Fullname = fullname;
            Email = email;
            CreatedAt = createdAt;
            IsActived = isActived;
        }
        private User(Guid id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool isActived, List<Role> roles)
        {
            Id = id;
            this.username = username;
            password = passwordHash;
            Fullname = fullname;
            Email = email;
            CreatedAt = createdAt;
            IsActived = isActived;
            Roles.AddRange(roles);
        }
        public static User Create(Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool IsActived)
        {
            return new User(
                Guid.NewGuid(),
                username,
                passwordHash,
                fullname,
                email,
                createdAt,
                IsActived
            );
        }
        public static User Reconstruct(Guid Id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool IsActived, List<Role>roles)
        {
            return new User(Id, username, passwordHash, fullname, email, createdAt, IsActived, roles);
        }
        public void ChangeNameAndSurname(Fullname fullname)
        {
            Fullname = fullname;
        }
        public void ChangePassword(Password password)
        {
            if (string.IsNullOrWhiteSpace(password.HashValue))
                throw new ArgumentException("you should fill in these fields with password");
            this.password = password;
        }
        public void ChangeEmail(Email newEmail)
        {
            Email = newEmail;
        }

    }
}
