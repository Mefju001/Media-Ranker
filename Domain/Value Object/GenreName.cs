using Domain.Base;

namespace Domain.Value_Object;

public record GenreName : ValueObject
{
    public string Value { get; init; }

    public GenreName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Genre name cannot be empty");

        Value = value.Trim();
    }

    public static implicit operator string(GenreName genreName) => genreName.Value;

    public static explicit operator GenreName(string value) => new(value);

    public override string ToString() => Value;
}