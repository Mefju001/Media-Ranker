using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class RoleMapper
    {
        public static RoleResponse ToResponse(Role role)
        {
            return new RoleResponse(role.role)
            {
                role = role.role
            };
        }
    }
}
