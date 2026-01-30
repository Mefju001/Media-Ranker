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
        public async Task<ERole?> GetByNameAsync(string roleName)
        {
            var result =  await appDbContext.Roles.FirstOrDefaultAsync(r => r.role.ToString() == roleName);
            return result?.role;
        }
    }
}
