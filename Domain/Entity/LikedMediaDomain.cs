namespace Domain.Entity
{
    public class LikedMediaDomain
    {
        public int id { get; private set; }
        public int userId { get; private set; }
        public int mediaId { get; private set; }
        public DateTime likedDate { get; private set; }
        private LikedMediaDomain(int userId, int mediaId) 
        {
            Validate(userId, mediaId);
            this.userId = userId;
            this.mediaId = mediaId;
            this.likedDate = DateTime.UtcNow;
        }
        private LikedMediaDomain() { }
        private LikedMediaDomain(int id, int userId, int mediaId, DateTime likedDate)
        {
            this.id = id;
            this.userId = userId;
            this.mediaId = mediaId;
            this.likedDate = likedDate;
        }
        public static LikedMediaDomain Create(int userId, int mediaId)
        {
            return new LikedMediaDomain(userId, mediaId);
        }
        private static void Validate(int userId, int mediaId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
            }
            if (mediaId <= 0)
            {
                throw new ArgumentException("Media ID must be a positive integer.", nameof(mediaId));
            }
        }
    }
}
