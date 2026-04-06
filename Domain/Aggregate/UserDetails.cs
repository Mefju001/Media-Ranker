using Domain.Base;
using Domain.Entity;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class UserDetails : AggregateRoot<Guid>,IAudited
{
    public Fullname Fullname { get; private set; } = default!;
    public Email email { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<LikedMedia> likedMedias = new();
    public IReadOnlyCollection<LikedMedia> LikedMedias => likedMedias.AsReadOnly();
    private UserDetails() { }

    public static UserDetails Create(Guid id,Fullname fullname, Email email)
    {
        return new UserDetails
        {
            Id = id,
            Fullname = fullname,
            email = email,
            IsActive = true,
            AuditInfo = new AuditInfo()
        };
    }

    public void AddLikedMedia(int movieId)
    {
        if (likedMedias.Any(lm => lm.MediaId.Equals(movieId)))
            throw new InvalidOperationException("Media is already liked.");
        likedMedias.Add(LikedMedia.Create(Id, movieId));
    }
    public void RemoveLikedMedia(int movieId)
    {
        var likedMedia = likedMedias.FirstOrDefault(lm => lm.MediaId.Equals(movieId));
        if (likedMedia == null)
            throw new InvalidOperationException("Media is not in liked list.");
        likedMedias.Remove(likedMedia);
    }

    public void UpdateProfile(Fullname fullname,Email email)
    {
        Fullname = fullname;
        this.email = email;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }

}