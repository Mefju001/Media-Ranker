using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.AddLiked
{
    public record AddLikedCommand(Guid UserId, int MediaId) : IRequest<LikedMediaResponse>;
}
