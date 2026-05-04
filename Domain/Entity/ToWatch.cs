using Domain.Base;


namespace Domain.Entity
{
    public class ToWatch:Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid MediaId { get; private set; }
        public DateTime LikedDate { get; private set; }
        private ToWatch() { }
        public static ToWatch Create(Guid userId, Guid mediaId, Guid? id = null)
        {
            var toWatch = new ToWatch
            {
                Id = id ?? Guid.NewGuid(),
                UserId = userId,
                MediaId = mediaId,
                LikedDate = DateTime.UtcNow
            };
            return toWatch;
        }
    }
}
