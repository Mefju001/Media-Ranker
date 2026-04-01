using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.GetAll
{
    public record GetAllQuery : IQuery<List<GameResponse>>;
}
