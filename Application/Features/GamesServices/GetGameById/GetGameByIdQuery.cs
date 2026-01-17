using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.GamesServices.GetGameById
{
    public record GetGameByIdQuery(int id) : IRequest<GameResponse?>;

}
