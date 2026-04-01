using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.LikedServices.GetAllLiked
{
    public record GetAllQuery : IQuery<List<LikedMediaResponse>>;
}
