using MediatR;

namespace Application.Features.ToWatch.Add
{
    public record AddCommand(Guid MediaId, Guid UserId) : IRequest<Unit>;
}