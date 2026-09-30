using Application.Features.Common.Interfaces;
using Application.Features.User.Common;

namespace Application.Features.User.GetUserExcludeAdmin
{
    public record GetUserExcludeAdminQuery : IQuery<List<UserDetailsResponse>>;
}