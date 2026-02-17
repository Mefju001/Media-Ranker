namespace Domain.Entity
{
    public class Director
    {
        public int Id { get; init; }
        public string name { get; set; }
        public string surname { get; set; }
        private Director() { }
        private Director(string name, string surname)
        {
            this.name = name;
            this.surname = surname;
        }
        public static Director Create(string name, string surname)
        {
            return new Director(name, surname);
        }
        public static void Validate(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be null or empty.");
        }
        public static Director Reconstruct(int id, string name, string surname)
        {
            return new Director(name, surname)
            {
                Id = id
            };
        }
    }
}
