using Domain.Enums;

namespace Domain.Entity
{
    public class RoleDomain
    {
        public int Id { get; private set; }
        public ERole role { get; private set; }
        protected RoleDomain() { }
        private RoleDomain(ERole role)
        {
            this.role = role;
        }
        public void SetRole(ERole role)
        {
            this.role = role;
        }
        public static RoleDomain Create(ERole role)
        {
            return new RoleDomain(role);
        }

    }
}
