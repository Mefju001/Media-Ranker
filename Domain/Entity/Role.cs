using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity
{
    public class Role
    {
        public Guid Id { get; private set; }
        public ERole role { get; private set; }
        protected Role() { }
        private Role(ERole role)
        {
            this.role = role;
        }
        public void SetRole(ERole role)
        {
            this.role = role;
        }
        public static Role Create(ERole role)
        {
            return new Role(role);
        }
        public static Role Reconstruct(Guid roleId,string name)
        {
            Enum.TryParse<ERole>(name, out var role);
            return new Role
            {
                Id = roleId,
                role = role
            };
        }
    }
}
