namespace Domain.Entity
{
    public class LikedMedia
    {
        public int id { get; private set; }
        public Guid userId { get; private set; }
        public int mediaId { get; private set; }
        public DateTime likedDate { get; private set; }
        private LikedMedia() { }
        private LikedMedia(Guid userId, int mediaId)
        {
            this.userId = userId;
            this.mediaId = mediaId;
            this.likedDate = DateTime.UtcNow;
        }
        private LikedMedia(int id, Guid userId, int mediaId, DateTime likedDate)
        {
            this.id = id;
            this.userId = userId;
            this.mediaId = mediaId;
            this.likedDate = likedDate;
        }
        public static LikedMedia Create(Guid userId, int mediaId)
        {
            return new LikedMedia(userId, mediaId);
        }
    }
}
