using MediatR;

namespace Application.Features.UserServices.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<Unit>;
}
