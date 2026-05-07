using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.User.ChangePassword
{
    public record ChangePasswordCommand(string newPassword, string confirmPassword, string oldPassword, Guid userId) : ICommand<Unit>;
}
