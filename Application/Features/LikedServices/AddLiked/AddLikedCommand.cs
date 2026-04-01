using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.LikedServices.AddLiked
{
    public record AddLikedCommand(Guid UserId, int MediaId) : ICommand<LikedMediaResponse>;
}
