using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.AddListOfGames
{
    public record AddListOfGamesCommand(List<GameRequest> games) : ICommand<List<GameResponse>>;
}
