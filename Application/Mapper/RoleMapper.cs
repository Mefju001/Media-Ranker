using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
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
