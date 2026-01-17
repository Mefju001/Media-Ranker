using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public record GetAllLikedByUserQuery(int userId) : IRequest<List<LikedMediaResponse>>;

}
