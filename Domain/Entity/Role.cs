using Domain.Enums;

namespace Domain.Entity
{
    public class Role
    {
        public int Id { get; private set; }
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

    }
}
