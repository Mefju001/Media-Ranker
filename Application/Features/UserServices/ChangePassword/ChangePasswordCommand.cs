using Application.Common.DTO.Request;
using MediatR;

namespace Application.Features.UserServices.ChangePassword
{
    public record ChangePasswordCommand(string newPassword, string confirmPassword, string oldPassword, Guid userId) : IRequest<Unit>;
}
