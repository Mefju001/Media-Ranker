using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;

namespace Application.Features.Liked.GetAll
{
    public record GetAllQuery : IQuery<List<LikedMediaResponse>>;
}
