using Application.Features.Common.Interfaces;
using Application.Features.User.Common;

namespace Application.Features.User.GetByName
{
    public record GetByNameQuery(string name) : IQuery<UserDetailsResponse?>;
}
