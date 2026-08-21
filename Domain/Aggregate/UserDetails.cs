using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class UserDetails : AggregateRoot<Guid>, IAudited
{
    public string Username { get; private set; } = default!;
    public Email Email { get; private set; } = null!;
    public Fullname Fullname { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<UserInteractions> userInteractions = new();
    public IReadOnlyCollection<UserInteractions> UserInteractions => userInteractions.AsReadOnly();
    private UserDetails() { }

    public static UserDetails Create(Guid? id, Fullname fullname, string username, Email email)
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
    public void SetTypeInteractions(Guid mediaId, ETypeInteractions? type)
    {
        var existing = userInteractions.FirstOrDefault(ui => ui.MediaId == mediaId);

        if (existing == null)
        {
            var newInteraction = Entity.UserInteractions.Create(Id, mediaId, type, null);
            userInteractions.Add(newInteraction);
            CleanupAndAudit(newInteraction);
        }
        else
        {
            if(existing.TypeInteractions == type)
            {
                throw new DomainException("TypeInteractions is already set to the same value.");
            }
            existing.UpdateTypeInteractions(type);
            CleanupAndAudit(existing);
        }
    }

    public void SetRatingVote(Guid mediaId, ERatingVote? vote)
    {
        var existing = userInteractions.FirstOrDefault(ui => ui.MediaId == mediaId);

        if (existing == null)
        {
            var newInteraction = Entity.UserInteractions.Create(Id, mediaId, null, vote);
            userInteractions.Add(newInteraction);
            CleanupAndAudit(newInteraction);
        }
        else
        {
            if (existing.RatingVote == vote)
            {
                throw new DomainException("RatingVote is already set to the same value.");
            }
            existing.UpdateRatingVote(vote);
            CleanupAndAudit(existing);
        }
    }
    public void SetInteraction(Guid mediaId, ETypeInteractions? type, ERatingVote? vote)
    {
        var existing = userInteractions.FirstOrDefault(ui => ui.MediaId == mediaId);
        if (existing == null)
        {
            var newInteraction = Entity.UserInteractions.Create(Id, mediaId, type, vote);
            userInteractions.Add(newInteraction);
            CleanupAndAudit(newInteraction);
        }
        else
        {
            existing.UpdateTypeInteractions(type);
            existing.UpdateRatingVote(vote);
            CleanupAndAudit(existing);
        }
    }

    private void CleanupAndAudit(UserInteractions interaction)
    {
        if (interaction.TypeInteractions == null && interaction.RatingVote == null)
        {
            userInteractions.Remove(interaction);
        }

        AuditInfo = AuditInfo.MarkAsUpdated();
    }
    public void RemoveInteraction(Guid mediaId)
    {
        var interaction = userInteractions.FirstOrDefault(ui =>
            ui.UserId == Id && ui.MediaId == mediaId);

        if (interaction == null)
        {
            throw new DomainException("Interaction not found for this user and media.");
        }

        userInteractions.Remove(interaction);
        AuditInfo = AuditInfo.MarkAsUpdated();
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
    public void UpdateUsername(string username)
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