using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.GetAllLiked
{
    public record GetAllQuery : IRequest<List<LikedMediaResponse>>;
}
