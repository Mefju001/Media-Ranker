using Domain.Base;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class UserDetails : AggregateRoot<Guid>, IAudited
{
    public Username Username { get; private set; } = default!;
    public Email Email { get; private set; } = null!;
    public Fullname Fullname { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<LikedMedia> likedMedias = new();
    public IReadOnlyCollection<LikedMedia> LikedMedias => likedMedias.AsReadOnly();
    private readonly List<ToWatch> toWatches = new();
    public IReadOnlyCollection<ToWatch> ToWatches => toWatches.AsReadOnly();
    private UserDetails() { }

    public static UserDetails Create(Guid? id, Fullname fullname, Username username, Email email)
    {
        return new UserDetails
        {
            Id = id ?? Guid.NewGuid(),
            Fullname = fullname,
            Username = username,
            Email = email,
            IsActive = true,
            AuditInfo = new AuditInfo()
        };
    }
    public void AddToWatch(Guid movieId)
    {
        if (toWatches.Any(tw => tw.MediaId.Equals(movieId)))
            throw new DomainException("Media is already in to-watch list.");
        toWatches.Add(ToWatch.Create(Id, movieId));
    }
    public void RemoveToWatch(Guid movieId)
    {
        var toWatch = toWatches.FirstOrDefault(tw => tw.MediaId.Equals(movieId));
        if (toWatch == null)
            throw new DomainException("Media is not in to-watch list.");
        toWatches.Remove(toWatch);
    }
    public void AddLikedMedia(Guid movieId)
    {
        if (likedMedias.Any(lm => lm.MediaId.Equals(movieId)))
            throw new DomainException("Media is already liked.");
        likedMedias.Add(LikedMedia.Create(Id, movieId));
    }
    public void RemoveLikedMedia(Guid movieId)
    {
        var likedMedia = likedMedias.FirstOrDefault(lm => lm.MediaId.Equals(movieId));
        if (likedMedia == null)
            throw new DomainException("Media is not in liked list.");
        likedMedias.Remove(likedMedia);
    }

    public void UpdateProfile(Fullname fullname)
    {
        Fullname = fullname;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }
    public void Deactivate()
    {
        IsActive = false;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }
    public void UpdateEmail(Email email)
    {
        this.Email = email;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }
    public void UpdateUsername(Username username)
    {
        this.Username = username;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }
    public void Activate()
    {
        IsActive = true;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }

}