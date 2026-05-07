using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Auth.Logout
{
    public record LogoutCommand(Guid UserId, string? jti) : ICommand<Unit>;
}
