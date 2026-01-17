using MediatR;

namespace Application.Features.AuthServices.Signup
{
    public record SignUpCommand : IRequest<SignUpResponse>;
}
