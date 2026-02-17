using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
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
                user.UserRoles.Select(ur => RoleMapper.ToResponse(ur)).ToList() ?? new List<RoleResponse>(),
                user.Reviews.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>());

        }
    }
}
