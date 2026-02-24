using Domain.Entity;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IRoleRepository
    {
        Task<ERole?> GetByNameAsync(string role);
    }
}
