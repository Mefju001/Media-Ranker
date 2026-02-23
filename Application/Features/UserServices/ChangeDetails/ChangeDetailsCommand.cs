using MediatR;

namespace Application.Features.UserServices.ChangeDetails
{
    public record ChangeDetailsCommand(Guid userId, string name, string surname, string email) : IRequest<Unit>;
}
