using Domain.Entity;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IRoleRepository
    {
        Task<RoleDomain?> GetByNameAsync(string role);
    }
}
