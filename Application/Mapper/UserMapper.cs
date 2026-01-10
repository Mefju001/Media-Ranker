using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(User user)
        {
            if (user is null) return null;
            return new UserResponse(
                user.Id,
                user.username,
                user.password,
                user.name,
                user.surname,
                user.email,
                user.UserRoles.Select(ur => RoleMapper.ToResponse(ur.Role)).ToList() ?? new List<RoleResponse>(),
                user.Reviews.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>());

        }
    }
}
