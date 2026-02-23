using MediatR;

namespace Application.Features.UserServices.DeleteUser
{
    public record DeleteUserCommand(Guid id) : IRequest<Unit>;
}
