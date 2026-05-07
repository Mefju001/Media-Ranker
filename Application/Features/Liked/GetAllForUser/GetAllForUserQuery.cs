using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;

namespace Application.Features.Liked.GetAllForUser
{
    public record GetAllForUserQuery(Guid userId) : IQuery<List<LikedMediaResponse>>;

}
