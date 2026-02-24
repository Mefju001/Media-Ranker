using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
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
        public async Task<ERole?> GetByNameAsync(string role)
        {
            throw new NotImplementedException();
            /*var result = await appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == role);
            return result;*/
        }
    }
}
