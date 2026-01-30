namespace Domain.Entity
{
    public class MediaStatsDomain
    {
        public int MediaId { get; set; }
        public double? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime? LastCalculated { get; set; }
        private MediaStatsDomain()
        {
            ReviewCount = 0;
            AverageRating = 0.0;
            LastCalculated = DateTime.UtcNow;
        }

        public static MediaStatsDomain Create()
        {
            return new MediaStatsDomain();
        }

        public void UpdateStatistics(double avgRat, int revCount)
        {
            if (revCount < 0) throw new ArgumentException("Review count cannot be negative");

            if (avgRat < 0 || avgRat > 10)
                throw new ArgumentOutOfRangeException(nameof(AverageRating), "Rating must be between 0 and 10");

            AverageRating = revCount > 0 ? avgRat : null;
            ReviewCount = revCount;
            LastCalculated = DateTime.UtcNow;
        }

        public void IncrementReviewCount()
        {
            ReviewCount++;
            LastCalculated = DateTime.UtcNow;
        }

    }
}
