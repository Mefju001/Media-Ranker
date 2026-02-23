using Domain.Entity;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string role);
    }
}
