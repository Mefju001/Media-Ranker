using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
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
