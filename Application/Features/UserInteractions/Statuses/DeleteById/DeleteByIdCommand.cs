using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.UserInteractions.Statuses.DeleteById
{
    public record DeleteByIdCommand(Guid UserId, Guid MediaId) : ICommand<Unit>;
}
