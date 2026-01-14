using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace Application.Features.LikedServices.AddLiked
{
    public record AddLikedCommand( int UserId, int MediaId) : IRequest<LikedMediaResponse>;
}
