using Domain.Base;

namespace Domain.Value_Object;

public record MediaStats : ValueObject
{
    public double AverageRating { get; init; }
    public int ReviewCount { get; init; }
    public DateTime LastCalculated { get; init; }

    public static MediaStats Empty() => new(0, 0);

    public MediaStats(double averageRating, int reviewCount)
    {
        Validate(averageRating, reviewCount);

        ReviewCount = reviewCount;
        AverageRating = reviewCount > 0 ? averageRating : 0.0;
        LastCalculated = DateTime.UtcNow;
    }

    private static void Validate(double avg, int count)
    {
        if (count < 0)
            throw new ArgumentException("Review count cannot be negative.");
        if (avg < 0 || avg > 10)
            throw new ArgumentOutOfRangeException(nameof(AverageRating), "Rating must be between 0 and 10.");
    }

    public MediaStats Update(double newAverage, int newCount)
    {
        return new MediaStats(newAverage, newCount);
    }

    public override string ToString() => $"{AverageRating:F1} ({ReviewCount} reviews)";
}