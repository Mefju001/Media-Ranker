using Application.Common.Interfaces;

namespace Application.Features.AuthServices.Signup
{
    public record SignUpCommand(string username, string email, string password, string name,
        string surname) : ICommand<SignUpResponse>;
}
