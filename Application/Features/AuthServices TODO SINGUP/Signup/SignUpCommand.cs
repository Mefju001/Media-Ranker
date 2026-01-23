using MediatR;

namespace Application.Features.AuthServices.Signup
{
    public record SignUpCommand(string username,string email,string password,string name,
        string surname) : IRequest<SignUpResponse>;
}
