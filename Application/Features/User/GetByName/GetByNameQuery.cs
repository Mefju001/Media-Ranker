using Application.Features.Common.DTO.Response;
using Application.Features.Common.Interfaces;

namespace Application.Features.User.GetByName
{
    public record GetByNameQuery(string name) : IQuery<UserDetailsResponse?>;
}
