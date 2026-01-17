using MediatR;

namespace Application.Features.UserServices.ChangeDetails
{
    public record ChangeDetailsCommand(int userId, string name, string surname, string email) : IRequest<Unit>;
}
