using Domain.Base;
using Domain.Exceptions;

namespace Domain.Aggregate;

public class Genre : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;

    private Genre() { }
    public static Genre Create(string name, Guid? id = null)
        => new Genre
        {
            Id = id ?? Guid.NewGuid(),
            Name = name??throw new DomainException(nameof(name)),
        };

    public void Update(string name)
        => Name = name;
}