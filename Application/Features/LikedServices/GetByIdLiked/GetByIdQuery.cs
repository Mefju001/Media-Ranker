using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public record GetByIdQuery(int id) : IQuery<LikedMediaResponse?>;
}
