using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public record GetByIdQuery(int id) : IRequest<LikedMediaResponse?>;
}
