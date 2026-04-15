using Domain.Base;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Entity;

public class Review : Entity<Guid>, IAudited
{
    public Guid MediaId { get; init; }
    public Guid UserId { get; init; }
    public Username Username { get; init; } = default!;
    public Rating Rating { get; private set; } = default!;
    public string Comment { get; private set; } = default!;
    public AuditInfo AuditInfo { get; private set; } = new();

    private Review() { }


    private Review(Guid id, Guid mediaId, Guid userId, Username username, Rating rating, string comment)

    {
        MediaId = mediaId;
        UserId = userId;
        Username = username;
        Rating = rating;
        Comment = comment;
    }

    public static Review Create(Rating rating, string comment, Guid mediaId, Guid userId, Username username)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.");

        return new Review(Guid.NewGuid(), mediaId, userId, username, rating, comment);
    }

    public void Update(Rating rating, string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.");

        Rating = rating;
        Comment = comment;

        AuditInfo.MarkAsUpdated();
    }
}