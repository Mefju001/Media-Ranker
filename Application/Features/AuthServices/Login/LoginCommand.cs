using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.Login
{
    public record LoginCommand(string username, string password) : ICommand<TokenResponse>;

}
