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
                user.username.Value,
                user.password.HashValue,
                user.Fullname.Name,
                user.Fullname.Surname,
                user.Email.Value,
                user.UserRoles.Select(ur => RoleMapper.ToResponse(ur)).ToList() ?? new List<RoleResponse>());
        }
    }
}
