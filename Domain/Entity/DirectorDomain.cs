namespace Domain.Entity
{
    public class DirectorDomain
    {
        public int Id { get; init; }
        public string name { get; set; }
        public string surname { get; set; }

        private DirectorDomain(string name, string surname)
        {
            this.name = name;
            this.surname = surname;
        }
        public static DirectorDomain Create(string name, string surname)
        {
            return new DirectorDomain(name, surname);
        }
        public static void Validate(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be null or empty.");
        }
        public static DirectorDomain Reconstruct(int id, string name, string surname)
        {
            return new DirectorDomain(name, surname)
            {
                Id = id
            };
        }
    }
}
