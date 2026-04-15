using Domain.Value_Object;

namespace Domain.Interfaces
{
    public interface IAudited
    {
        AuditInfo AuditInfo { get; }
    }
}
