using MediatR;

namespace Application.Features.ToWatchServices.Add
{
    public record AddCommand(Guid MediaId, Guid UserId) : IRequest<Unit>;
}