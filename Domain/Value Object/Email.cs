using Domain.Base;

namespace Domain.Value_Object;

public record Email:ValueObject
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        var trimmedEmail = value.Trim().ToLower();

        if (!trimmedEmail.Contains("@") || !trimmedEmail.Contains("."))
            throw new ArgumentException("Invalid email format.");

        Value = trimmedEmail;
    }

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => new(value);

    public override string ToString() => Value;
}