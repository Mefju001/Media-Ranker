using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.GetGameById
{
    public record GetGameByIdQuery(int id) : IQuery<GameResponse?>;

}
