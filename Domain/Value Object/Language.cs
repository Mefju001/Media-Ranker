using Domain.Base;
using Domain.Exceptions;

namespace Domain.Value_Object;

public record Language : ValueObject
{
    public string Value { get; init; }

    public Language(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Language cannot be empty.");

        Value = value.Trim();
    }

    public static implicit operator string(Language lang) => lang.Value;

    public static explicit operator Language(string value) => new(value);

    public override string ToString() => Value;
}