using Domain.Value_Object;

namespace Domain.Entity
{
    public class Review
    {
        public int Id { get; init; }
        public Rating Rating { get; private set; }
        public string Comment { get; private set; }
        public int MediaId { get; init; }
        public int UserId { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; private set; } = DateTime.UtcNow;
        private Review() { }
        private Review(Rating rating, string comment)
        {
            Validate(rating, comment);
            Rating = rating;
            Comment = comment;
        }
        public static Review Create(Rating rating, string comment, int MediaId, int UserId)
        {
            return new Review(rating, comment)
            {
                MediaId = MediaId,
                UserId = UserId
            };
        }
        public void Update(Rating rating, string comment)
        {
            Validate(rating, comment);
            Rating = rating;
            Comment = comment;
            LastModifiedAt = DateTime.UtcNow;
        }
        public static void Validate(Rating rating, string comment)
        {
            if (rating.value < 1 || rating.value > 10)
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
