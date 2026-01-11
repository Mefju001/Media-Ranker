using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Games.GetMovieById
{
    public record GetGameByIdQuery(int id) : IRequest<GameResponse?>;

}
