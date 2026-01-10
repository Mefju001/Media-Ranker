using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Games.AddListOfGames
{
    public record AddListOfGamesCommand(List<GameRequest> requests) : IRequest<List<GameResponse>>;
}
