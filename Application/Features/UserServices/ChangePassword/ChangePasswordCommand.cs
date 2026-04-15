using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.UserServices.ChangePassword
{
    public record ChangePasswordCommand(string newPassword, string confirmPassword, string oldPassword, Guid userId) : ICommand<Unit>;
}
