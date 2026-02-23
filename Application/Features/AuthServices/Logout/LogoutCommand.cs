using MediatR;

namespace Application.Features.AuthServices.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest;
}
