using MediatR;

namespace Application.Features.UserServices.ChangePassword
{
    public record ChangePasswordCommand(string newPassword, string oldPassword, string confirmPassword, Guid userId) : IRequest<Unit>;
}
