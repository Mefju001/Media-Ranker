using Domain.Base;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Genre:AggregateRoot<int>
{
    public GenreName Name { get; private set; } = default!;

    private Genre() { }
    public static Genre Create(string name, int id = 0)
        => new Genre
        {
            Id = id,
            Name = new GenreName(name)
        };

    public void Update(string name)
        => Name = new GenreName(name);
}