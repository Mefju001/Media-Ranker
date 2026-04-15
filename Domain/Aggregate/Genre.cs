using Domain.Base;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Genre : AggregateRoot<Guid>
{
    public GenreName Name { get; private set; } = default!;

    private Genre() { }
    public static Genre Create(string name, Guid? id = null)
        => new Genre
        {
            Id = id ?? Guid.NewGuid(),
            Name = new GenreName(name)
        };

    public void Update(string name)
        => Name = new GenreName(name);
}