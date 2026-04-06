using Domain.Base;

namespace Domain.Entity;

public class LikedMedia : Entity<int>
{
    public Guid UserId { get; private set; }
    public int MediaId { get; private set; }
    public DateTime LikedDate { get; private set; }

    private LikedMedia() { }

    public static LikedMedia Create(Guid userId, int mediaId, int id = 0)
    {
        var liked = new LikedMedia
        {
            Id = id,
            UserId = userId,
            MediaId = mediaId,
            LikedDate = DateTime.UtcNow
        };

       /* if (id == 0)
            liked._domainEvents.Add(new MediaLiked(userId, mediaId));*/

        return liked;
    }

}