using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(User user)
        {
            if (user is null) return null;
            return new UserResponse(
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
