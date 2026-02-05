namespace Domain.Entity
{
    public class ReviewDomain
    {
        public int Id { get; init; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public int MediaId { get; init; }
        public int UserId { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; private set; } = DateTime.UtcNow;
        private ReviewDomain() { }
        private ReviewDomain(int rating, string comment)
        {
            Validate(rating, comment);
            Rating = rating;
            Comment = comment;
        }
        public static ReviewDomain Create(int rating, string comment, int MediaId, int UserId)
        {
            return new ReviewDomain(rating, comment)
            {
                MediaId = MediaId,
                UserId = UserId
            };
        }
        public void Update(int rating, string comment)
        {
            Validate(rating, comment);
            Rating = rating;
            Comment = comment;
            LastModifiedAt = DateTime.UtcNow;
        }
        public static void Validate(int rating, string comment)
        {
            if (rating < 1 || rating > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 10.");
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Comment cannot be null or empty.", nameof(comment));
            }
        }
    }
}
