using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.GamesServices.GetAll
{
    public record GetAllQuery : IRequest<List<GameResponse>>;
}
