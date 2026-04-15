using Application.Common.DTO.Response;

namespace Application.Mapper
{
    public static class RoleMapper
    {
        public static RoleResponse ToResponse(string role)
        {
            return new RoleResponse(role)
            {
                name = role
            };
        }
    }
}
