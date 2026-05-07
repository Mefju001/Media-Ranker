using Application.Features.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Features.Common.Mapper
{
    public static class UserMapper
    {
        public static UserDetailsResponse ToResponse(UserDetails user)
        {
            if (user is null) return null;
            return new UserDetailsResponse(
                user.Id,
                user.Username.Value,
                user.Email.ToString(),
                user.Fullname.Name,
                user.Fullname.Surname
                );
        }
    }
}
