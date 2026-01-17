using MediatR;

namespace Application.Features.AuthServices.Logout
{
    public record LogoutCommand(string stringUserId) : IRequest;
}
