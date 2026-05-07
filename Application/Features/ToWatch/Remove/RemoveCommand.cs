using MediatR;

namespace Application.Features.ToWatch.Remove
{
    public record RemoveCommand(Guid MovieId, Guid UserId) : IRequest<Unit>;
}