using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public record GetAllLikedByUserQuery(Guid userId) : IQuery<List<LikedMediaResponse>>;

}
