using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.UserServices.GetBy
{
    public record GetUserByNameQuery(string name) : IQuery<UserDetailsResponse?>;
}
