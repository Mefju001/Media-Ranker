using MediatR;

namespace Application.Features.ToWatchServices.Remove
{
    public record RemoveCommand(Guid MovieId, Guid UserId) : IRequest<Unit>;
}