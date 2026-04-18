using Domain.Base;
using Domain.Exceptions;

namespace Domain.Value_Object;

public record ReleaseDate : ValueObject
{
    public DateTime Value { get; init; }

    public ReleaseDate(DateTime value)
    {
        if (value.Date > DateTime.UtcNow.Date)
            throw new DomainException("Release date cannot be in the future.");

        if (value.Year < 1800)
            throw new DomainException("Release date is too far in the past.");

        Value = value.Date;
    }

    public static implicit operator DateTime(ReleaseDate releaseDate) => releaseDate.Value;

    public static explicit operator ReleaseDate(DateTime value) => new(value);

    public override string ToString() => Value.ToShortDateString();
}