namespace Domain.Entity
{
    public class GenreDomain
    {
        public int Id { get; init; }
        public string name { get; private set; }
        private GenreDomain() { }
        private GenreDomain(string name)
        {
            Validate(name);
            this.name = name;
        }
        public static GenreDomain Create(string name)
        {
            return new GenreDomain(name);
        }
        public void Update(string name)
        {
            Validate(name);
            this.name = name;
        }
        public static void Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Genre name cannot be null or empty.", nameof(name));
            }
        }
        public static GenreDomain Reconstruct(int Id, string name)
        {
            Validate(name);
            return new GenreDomain(name) { Id = Id };
        }
    }
}
