using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.LikedServices.GetAllLiked
{
    public record GetAllQuery : IQuery<List<LikedMediaResponse>>;
}
