using Domain.Enums;

namespace Domain.Entity
{
    public class RoleDomain
    {
        public int Id { get; private set; }
        public ERole role { get; private set; }
        private RoleDomain(ERole role)
        {
            this.role = role;
        }
        private RoleDomain() { }
        public static RoleDomain Create(ERole role)
        {
            return new RoleDomain(role);
        }

    }
}
