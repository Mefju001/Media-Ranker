using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.AuthServices.Login
{
    public record LoginCommand(string username, string password) : ICommand<TokenResponse>;

}
