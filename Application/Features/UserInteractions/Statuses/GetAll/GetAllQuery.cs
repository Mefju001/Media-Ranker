using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Statuses.Common;
using Domain.Enums;

namespace Application.Features.UserInteractions.Statuses.GetAll
{
    public record GetAllQuery(Guid UserId, ERatingVote? ERatingVote, ETypeInteractions? ETypeInteractions):IQuery<List<UserInteractionsResponse>>;
}
