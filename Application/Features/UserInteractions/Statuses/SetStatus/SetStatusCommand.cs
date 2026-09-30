using Application.Features.Common.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Features.UserInteractions.Statuses.SetStatus
{
    public record SetStatusCommand(Guid userId, Guid mediaId, ERatingVote? ratingVote, ETypeInteractions? typeInteractions) : ICommand<Unit>;
}
