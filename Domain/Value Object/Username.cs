using Domain.Base;

namespace Domain.Value_Object;

public record Username : ValueObject
{
    public string Value { get; init; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username cannot be empty", nameof(value));

        if (value.Length < 3)
            throw new ArgumentException("Username is too short", nameof(value));

        Value = value;
    }

    public static implicit operator string(Username username) => username.Value;

    public static explicit operator Username(string value) => new(value);

    public override string ToString() => Value;
}