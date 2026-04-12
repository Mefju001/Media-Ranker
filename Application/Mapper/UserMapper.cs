using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Mapper
{
    public static class UserMapper
    {
        public static UserDetailsResponse ToResponse(UserDetails user)
        {
            if (user is null) return null;
            return new UserDetailsResponse(
                user.Id,
                user.Fullname.Name,
                user.Fullname.Surname
                );
        }
    }
}
