using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.GamesServices.GetGameById
{
    public record GetGameByIdQuery(Guid id) : IQuery<GameResponse?>;

}
