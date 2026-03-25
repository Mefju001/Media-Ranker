using Domain.Value_Object;

namespace Domain.Entity
{
    public class Genre
    {
        public int Id { get; init; }
        public GenreName name { get; private set; }
        private Genre() { }
        private Genre(GenreName name)
        {
            this.name = name;
        }
        public static Genre Create(string name)
                => new(new GenreName(name));

        public void Update(string name)
            => this.name = new GenreName(name);

        public static Genre Reconstruct(int id, string name)
            => new(new GenreName(name)) { Id = id };
    }
}
