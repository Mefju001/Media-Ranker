using Domain.Base;

namespace Domain.Aggregate;

public class Director:AggregateRoot<int>
{
    public string Name { get; private set; } = default!;
    public string Surname { get; private set; } = default!;

    private Director() { }
    public static Director Create(string name, string surname, int id = 0)
    {
        Validate(name, surname);
        return new Director
        {
            Id = id,
            Name = name,
            Surname = surname
        };
    }

    public void Update(string name, string surname)
    {
        Validate(name, surname);
        Name = name;
        Surname = surname;
    }

    private static void Validate(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Name and Surname are required.");
    }
}