using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Games.GetAll
{
    public record GetAllQuery : IRequest<List<GameResponse>>;
}
