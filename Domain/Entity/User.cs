using Domain.Enums;
using System.Xml.Linq;

namespace Domain.Entity
{
    public class User
    {
        public int Id { get; private set; }
        public string username { get; private set; }
        public string password { get; private set; }
        public string name { get; private set; }
        public string surname { get; private set; }
        public string email { get; private set; }
        private readonly List<Role> _userRoles = new();
        public virtual IReadOnlyCollection<Role> UserRoles => _userRoles.AsReadOnly();

        private readonly List<Review> _reviews = new();
        public virtual IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        private User() { }
        private User(string username, string passwordHash, string name, string surname, string email)
        {
            this.username = username;
            password = passwordHash;
            this.name = name;
            this.surname = surname;
            this.email = email;
        }
        public static User Create(string username, string passwordHash, string name, string surname, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Invalid email format.");

            return new User(
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
        public void AddReview(Review review)
        {
            if (_reviews.Any(r => r.MediaId == review.MediaId))
                throw new InvalidOperationException("User has already reviewed this media.");
            _reviews.Add(review);
        }
        public void AddRole(Role roleDomain)
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
