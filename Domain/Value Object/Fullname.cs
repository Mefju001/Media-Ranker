using Domain.Base;

namespace Domain.Value_Object
{
    public record Fullname : ValueObject
    {
        public string Name { get; init; }
        public string Surname { get; init; }

        public Fullname(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname cannot be empty.");

            Name = name.Trim();
            Surname = surname.Trim();
        }

        public override string ToString() => $"{Name} {Surname}";
    }
}
