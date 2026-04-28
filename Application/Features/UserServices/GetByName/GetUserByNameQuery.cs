using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.UserServices.GetBy
{
    public record GetUserByNameQuery(string name) : IQuery<UserDetailsResponse?>;
}
