using Application.Features.Common.Interfaces;
using Application.Features.Common.Models;

namespace Application.Features.Recommendation.GetForUser
{
    public record GetForUserQuery(Guid UserId) : IQuery<List<MediaResponse>>;
}
