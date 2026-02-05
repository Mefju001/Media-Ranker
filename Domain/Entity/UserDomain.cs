using Domain.Enums;
using System.Xml.Linq;

namespace Domain.Entity
{
    public class UserDomain
    {
        public int Id { get; private set; }
        public string username { get; private set; }
        public string password { get; private set; }
        public string name { get; private set; }
        public string surname { get; private set; }
        public string email { get; private set; }
        private readonly List<RoleDomain> _userRoles = new();
        public virtual IReadOnlyCollection<RoleDomain> UserRoles => _userRoles.AsReadOnly();

        private readonly List<ReviewDomain> _reviews = new();
        public virtual IReadOnlyCollection<ReviewDomain> Reviews => _reviews.AsReadOnly();
        private UserDomain() { }
        private UserDomain(string username, string passwordHash, string name, string surname, string email)
        {
            this.username = username;
            password = passwordHash;
            this.name = name;
            this.surname = surname;
            this.email = email;
        }
        public static UserDomain Create(string username, string passwordHash, string name, string surname, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Invalid email format.");

            return new UserDomain(
                username,
                passwordHash,
                name,
                surname,
                email
            );
        }
        public void ChangeNameAndSurname(string newName, string newSurname)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(newSurname)) throw new ArgumentException("Surname cannot be empty.");
            name = newName;
            surname = newSurname;
        }
        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash)) throw new ArgumentException("Password cannot be empty.");
            password = newPasswordHash;
        }
        public void ChangeEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                throw new ArgumentException("Invalid email format.");
            email = newEmail;
        }
        public void AddReview(ReviewDomain review)
        {
            if (_reviews.Any(r => r.MediaId == review.MediaId))
                throw new InvalidOperationException("User has already reviewed this media.");
            _reviews.Add(review);
        }
        public void AddRole(RoleDomain roleDomain)
        {
            if (_userRoles.Any(r => r.role == roleDomain.role)) return;
            _userRoles.Add(roleDomain);
        }
        public void RemoveRole(ERole role)
        {
            var roleToRemove = _userRoles.FirstOrDefault(r => r.role == role);
            if (roleToRemove != null)
            {
                _userRoles.Remove(roleToRemove);
            }
        }
    }
}
