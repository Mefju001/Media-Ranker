using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.AuthServices.Login
{
    public record LoginCommand(string username, string password) : IRequest<TokenResponse>;

}
