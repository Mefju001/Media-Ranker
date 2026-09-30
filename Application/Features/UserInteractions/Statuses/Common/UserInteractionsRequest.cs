using Domain.Enums;

namespace Application.Features.UserInteractions.Statuses.Common
{
    public record UserInteractionsRequest(Guid MediaId, ERatingVote? RatingVote, ETypeInteractions? TypeInteractions);
}

