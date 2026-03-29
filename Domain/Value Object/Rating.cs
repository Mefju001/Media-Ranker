using Domain.Base;

namespace Domain.Value_Object;

public record Rating:ValueObject
{
    public int Value { get; init; }

    public Rating(int value)
    {
        if (value < 1 || value > 10)
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 10.");

        Value = value;
    }

    public static implicit operator int(Rating rating) => rating.Value;

    public static explicit operator Rating(int value) => new(value);

    public override string ToString() => $"{Value}/10";
}