using Application.Common.Interfaces;
using Domain.Enums;
using Infrastructure.DBModels;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Persistence.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly RoleManager<RoleModel> roleManager;
        public RoleRepository(AppDbContext appDbContext, RoleManager<RoleModel> roleManager)
        {
            this.appDbContext = appDbContext;
            this.roleManager = roleManager;
        }
        public async Task<ERole?> GetByNameAsync(string role)
        {
            var result = await roleManager.FindByNameAsync(role);
            var finalRole = Enum.TryParse<ERole>(result?.Name, out ERole erole) ? erole : (ERole?)null;
            return finalRole;
        }
    }
}
