using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.UserServices.GetBy
{
    public record GetUserByNameQuery(string name) : IRequest<UserResponse?>;
}
