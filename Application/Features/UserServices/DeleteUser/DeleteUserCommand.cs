using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.UserServices.DeleteUser
{
    public record DeleteUserCommand(Guid id) : ICommand<Unit>;
}
