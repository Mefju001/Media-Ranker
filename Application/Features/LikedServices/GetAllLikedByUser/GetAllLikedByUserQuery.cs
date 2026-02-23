using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public record GetAllLikedByUserQuery(Guid userId) : IRequest<List<LikedMediaResponse>>;

}
