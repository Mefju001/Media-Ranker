using Domain.Base;
using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;
using Microsoft.VisualBasic;

namespace Domain.Aggregate;

public class UserDetails : AggregateRoot<Guid>, IAudited
{
    public Username Username { get; private set; } = default!;
    public Email Email { get; private set; } = null!;
    public Fullname Fullname { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<UserInteractions> userInteractions = new();
    public IReadOnlyCollection<UserInteractions> UserInteractions => userInteractions.AsReadOnly();
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
    public void SetTypeInteractions(Guid mediaId, ETypeInteractions? type)
    {
        var interaction = GetOrAdd(mediaId);
        interaction.UpdateTypeInteractions(type);

        CleanupAndAudit(interaction);
    }

    public void SetRatingVote(Guid mediaId, ERatingVote? vote)
    {
        var interaction = GetOrAdd(mediaId);
        interaction.UpdateRatingVote(vote);

        CleanupAndAudit(interaction);
    }

    public void SetInteraction(Guid mediaId, ETypeInteractions? type, ERatingVote? vote)
    {
        var interaction = GetOrAdd(mediaId);
        interaction.UpdateInteraction(type, vote);

        CleanupAndAudit(interaction);
    }


    private UserInteractions GetOrAdd(Guid mediaId)
    {
        var existing = userInteractions.FirstOrDefault(ui =>
            ui.Id.UserId == this.Id && ui.Id.MediaId == mediaId);

        if (existing == null)
        {
            existing = Entity.UserInteractions.Create(this.Id, mediaId, null, null);
            userInteractions.Add(existing);
        }
        return existing;
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
            ui.Id.UserId == this.Id && ui.Id.MediaId == mediaId);

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