using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Database.DBModels
{
    public class RoleModel : IdentityRole<Guid>
    {
        public RoleModel() : base() { }
        public RoleModel(string roleName) : base(roleName) { }
    }
}
