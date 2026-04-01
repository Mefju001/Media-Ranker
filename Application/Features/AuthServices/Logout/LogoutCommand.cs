using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.Logout
{
    public record LogoutCommand(Guid UserId, string? jti) : ICommand<Unit>;
}
