using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.GamesServices.AddListOfGames
{
    public record AddListOfGamesCommand(List<GameRequest> requests) : IRequest<List<GameResponse>>;
}
