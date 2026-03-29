using Domain.Base;
using Domain.Entity;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class User : AggregateRoot<Guid>,IAudited
{
    public Username Username { get; private set; } = default!;
    public Password Password { get; private set; } = default!;
    public Fullname Fullname { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private readonly HashSet<ERole> _roles = new();
    public IReadOnlyCollection<ERole> UserRoles => _roles.ToList();
    public AuditInfo AuditInfo { get; private set; } = new();

    private User() { }

    public static User Create(Username username, Password password, Fullname fullname, Email email,
        Guid? id = null, bool isActive = true, IEnumerable<ERole>? roles = null)
    {
        var user = new User
        {
            Id = id ?? Guid.NewGuid(),
            Username = username,
            Password = password,
            Fullname = fullname,
            Email = email,
            AuditInfo = new AuditInfo(),
            IsActive = isActive
        };

        if (roles != null)
            foreach (var role in roles) user._roles.Add(role);
        else
            user._roles.Add(ERole.User);

        return user;
    }


    public void UpdateProfile(Fullname fullname, Email email)
    {
        Fullname = fullname;
        Email = email;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }

    public void ChangePassword(Password password)
    {
        if (string.IsNullOrWhiteSpace(password.HashValue))
            throw new ArgumentException("Password hash cannot be empty.");
        Password = password;
        AuditInfo = AuditInfo.MarkAsUpdated();
    }


    public void PromotionToManager()
    {
        if (_roles.Contains(ERole.Admin)) throw new InvalidOperationException("User is already an Admin.");
        _roles.Add(ERole.Manager);
    }

    public void PromotionToAdmin()
    {
        if (!_roles.Contains(ERole.Manager)) throw new InvalidOperationException("Must be a Manager first.");
        _roles.Add(ERole.Admin);
    }

    public void DemotionFromAdmin() => _roles.Remove(ERole.Admin);

    public void DemotionFromManager()
    {
        if (_roles.Contains(ERole.Admin)) throw new InvalidOperationException("Demote from Admin first.");
        _roles.Remove(ERole.Manager);
    }
}