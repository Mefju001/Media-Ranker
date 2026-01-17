using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.AddLiked
{
    public record AddLikedCommand(int UserId, int MediaId) : IRequest<LikedMediaResponse>;
}
