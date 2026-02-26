namespace Domain.Value_Object
{
    public record MediaStats
    {
        public double? AverageRating { get; init; }
        public int ReviewCount { get; init; }
        public DateTime? LastCalculated { get; init; }
        public MediaStats(double? AverageRating, int ReviewCount)
        {
            this.Validate(AverageRating, ReviewCount);
            this.AverageRating = ReviewCount > 0 ? AverageRating : 0.0;
            this.ReviewCount = ReviewCount;
            LastCalculated = DateTime.UtcNow;
        }
        private void Validate(double? avg, int count)
        {
            if (count < 0) throw new ArgumentException("Review count cannot be negative");

            if (avg < 0 || avg > 10)
                throw new ArgumentOutOfRangeException(nameof(AverageRating), "Rating must be between 0 and 10");

        }
    }
}
