using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;

namespace Application.Features.Liked.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<LikedResponse?>;
}
