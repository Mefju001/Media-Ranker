using Application.Features.User.Common;

namespace Application.Features.User.GetUserExcludeAdmin
{
    public record UserDetailsWithRoleResponse(UserDetailsResponse UserDetailsResponse, List<string>roles);
}
