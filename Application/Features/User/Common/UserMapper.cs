using Domain.Aggregate;

namespace Application.Features.User.Common
{
    public static class UserMapper
    {
        public static UserDetailsResponse ToResponse(UserDetails user)
        {
            if (user is null) return null;
            return new UserDetailsResponse(
                user.Id,
                user.Username,
                user.Email.ToString(),
                user.Fullname.FirstName,
                user.Fullname.LastName
                );
        }
    }
}
