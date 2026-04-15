using Domain.Base;

namespace Domain.Value_Object;

public record AuditInfo : ValueObject
{
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public AuditInfo()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    private AuditInfo(DateTime createdAt, DateTime? updatedAt)
    {
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public AuditInfo MarkAsUpdated() => this with { UpdatedAt = DateTime.UtcNow };
}