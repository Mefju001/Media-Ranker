using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class RoleRepository: IRoleRepository
    {
        private readonly AppDbContext appDbContext;
        public RoleRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<RoleDomain?> GetByNameAsync(string role)
        {
            Enum.TryParse<ERole>(role, out var stringRole);
            if (stringRole == default)
            {
                return null;
            }
            var result = await appDbContext.Roles.FirstOrDefaultAsync(r => r.role == stringRole);
            return result;
        }
    }
}
