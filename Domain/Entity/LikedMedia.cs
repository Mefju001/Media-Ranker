using Domain.Base;

namespace Domain.Entity;

public class LikedMedia : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public Guid MediaId { get; private set; }
    public DateTime LikedDate { get; private set; }

    private LikedMedia() { }

    public static LikedMedia Create(Guid userId, Guid mediaId, Guid? id = null)
    {
        var liked = new LikedMedia
        {
            Id = id ?? Guid.NewGuid(),
            UserId = userId,
            MediaId = mediaId,
            LikedDate = DateTime.UtcNow
        };

        /* if (id == 0)
             liked._domainEvents.Add(new MediaLiked(userId, mediaId));*/

        return liked;
    }

}