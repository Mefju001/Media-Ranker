using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public record GetByIdQuery(Guid id) : IQuery<LikedMediaResponse?>;
}
