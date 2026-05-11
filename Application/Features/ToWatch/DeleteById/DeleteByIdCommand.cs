using MediatR;

namespace Application.Features.ToWatch.DeleteById
{
    public record DeleteByIdCommand(Guid MovieId, Guid UserId) : IRequest<Unit>;
}