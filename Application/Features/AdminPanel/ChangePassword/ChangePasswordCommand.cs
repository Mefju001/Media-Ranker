using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.AdminPanel.ChangePassword
{
    public record ChangePasswordCommand(Guid userId, string Password) : ICommand<Unit>;
}
