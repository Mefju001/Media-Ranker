using Domain.Enums;
using Domain.Value_Object;
using System.Runtime.CompilerServices;


namespace Domain.Entity
{
    public class User
    {
        public Guid Id { get; private set; }
        public Username Username { get; private set; }
        public Password Password { get; private set; }
        public Fullname Fullname { get; private set; }
        public CreatedAt CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public Email Email { get; private set; }

        private readonly HashSet<ERole> Roles = new();
        public virtual IReadOnlyCollection<ERole> UserRoles => Roles.ToList();

        private User() { }
        private User(Guid id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool isActived)
        {
            Id = id;
            Username = username;
            Password = passwordHash;
            Fullname = fullname;
            Email = email;
            CreatedAt = createdAt;
            IsActive = isActived;
        }
        private User(Guid id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool isActived, List<ERole> roles)
        {
            Id = id;
            Username = username;
            Password = passwordHash;
            Fullname = fullname;
            Email = email;
            CreatedAt = createdAt;
            IsActive = isActived;
            Roles = roles.ToHashSet();
        }
        public static User Create(Username username, Password passwordHash, Fullname fullname, Email email)
        {
            var user = new User(
                Guid.NewGuid(),
                username,
                passwordHash,
                fullname,
                email,
                new CreatedAt(DateTime.UtcNow),
                true
            );

            user.Roles.Add(ERole.User);
            return user;
        }
        public void Update(string name, string surname, Email email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("you should fill in these fields with name and surname");
            Fullname = new Fullname(name, surname);
            Email = email;
        }
        public static User Reconstruct(Guid Id, Username username, Password passwordHash, Fullname fullname, Email email, CreatedAt createdAt, bool IsActived, List<ERole> roles)
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
            Password = password;
        }
        public void ChangeEmail(Email newEmail)
        {
            Email = newEmail;
        }
        public void PromotionToManager()
        {
            if (Roles.Contains(ERole.Admin))
            {
                throw new InvalidOperationException("Nie można awansować użytkownika, który już jest Adminem.");
            }
            Roles.Add(ERole.Manager);
        }
        public void PromotionToAdmin()
        {
            if (!Roles.Contains(ERole.Manager))
            {
                throw new InvalidOperationException("Nie można awansować użytkownika do Admina bez posiadania roli Manager.");
            }
            Roles.Add(ERole.Admin);
        }
        public void DemotionFromAdmin()
        {
            if (!Roles.Contains(ERole.Admin))
            {
                throw new InvalidOperationException("Nie można zdegradować użytkownika, który nie jest Adminem.");
            }
            Roles.Remove(ERole.Admin);
        }
        public void DemotionFromManager()
        {
            if (!Roles.Contains(ERole.Manager))
            {
                throw new InvalidOperationException("Nie można zdegradować użytkownika, który nie jest Managerem.");
            }
            if (Roles.Contains(ERole.Admin))
            {
                throw new InvalidOperationException("Nie można zdegradować użytkownika z roli Manager, ponieważ posiada on rolę Admin. Najpierw zdegraduj go z roli Admin.");
            }
            Roles.Remove(ERole.Manager);
        }
        public void RemoveRole(ERole role)
        {
            if (role == ERole.User)
            {
                throw new InvalidOperationException("Nie można odebrać podstawowej roli User.");
            }
            Roles.Remove(role);
        }
    }
}
