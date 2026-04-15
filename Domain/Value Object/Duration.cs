using Domain.Base;

namespace Domain.Value_Object;

public record Duration : ValueObject
{
    public TimeSpan Value { get; init; }

    private Duration() { }

    public Duration(TimeSpan value)
    {
        if (value.TotalMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Duration must be greater than 0.");

        if (value.TotalHours > 12)
            throw new ArgumentOutOfRangeException(nameof(value), "Duration cannot exceed 12 hours.");

        Value = value;
    }

    public static Duration FromMinutes(int minutes) => new(TimeSpan.FromMinutes(minutes));

    public static implicit operator TimeSpan(Duration duration) => duration.Value;

    public static explicit operator int(Duration duration) => (int)duration.Value.TotalMinutes;

    public override string ToString() => $"{(int)Value.TotalHours}h {Value.Minutes}m";
}