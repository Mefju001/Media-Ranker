using Domain.Base;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Entity;

public class Review:Entity<int>,IAudited
{
    public int MediaId { get; init; }
    public Guid UserId { get; init; }
    public Username Username { get; init; } = default!;
    public Rating Rating { get; private set; } = default!;
    public string Comment { get; private set; } = default!;
    public AuditInfo AuditInfo { get; private set; } = new();

    private Review() { }


    private Review(int id, int mediaId, Guid userId, Username username, Rating rating, string comment)
        
    {
        MediaId = mediaId;
        UserId = userId;
        Username = username;
        Rating = rating;
        Comment = comment;
    }

    public static Review Create(Rating rating, string comment, int mediaId, Guid userId, Username username)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.");

        
        return new Review(0, mediaId, userId, username, rating, comment);
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