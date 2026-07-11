using Application.Features.Common.Interfaces;

namespace Application.Features.Auth.Login
{
    public record LoginCommand(string username, string password) : ICommand<LoginResponse>;

}
