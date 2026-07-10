using Application.Features.Common.Interfaces;

namespace Application.Features.Auth.Signup
{
    public record SignUpCommand(string username, string email, string password, string name, string surname) : ICommand<SignUpResponse>;
}
