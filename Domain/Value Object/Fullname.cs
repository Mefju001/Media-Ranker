using Domain.Exceptions;

namespace Domain.Value_Object
{
    public record Fullname(string FirstName, string LastName)
    {
        private Fullname() : this(default!, default!) { }

        public static Fullname Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("First name and last name cannot be empty.");

            return new Fullname(firstName.Trim(), lastName.Trim());
        }
    }
}
