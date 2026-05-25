using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;

namespace Application.Features.Watching.GetAll
{
    public record GetAllQuery(Guid UserId) : IQuery<List<LikedResponse>>;
}
