using Domain.Base;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Director : AggregateRoot<Guid>
{
    public Fullname fullname { get; private set; } = default!;

    private Director() { }
    public static Director Create(string name, string surname, Guid? id = null)
    {
        return new Director
        {
            Id = id ?? Guid.NewGuid(),
            fullname = Fullname.Create(name, surname)
        };
    }

    public void Update(string name, string surname)
    {
        fullname = Fullname.Create(name, surname);
    }

}