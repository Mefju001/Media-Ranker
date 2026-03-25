using Application.Common.DTO.Response;
using Domain.Enums;

namespace Application.Mapper
{
    public static class RoleMapper
    {
        public static RoleResponse ToResponse(ERole role)
        {
            return new RoleResponse(role)
            {
                name = role
            };
        }
    }
}
