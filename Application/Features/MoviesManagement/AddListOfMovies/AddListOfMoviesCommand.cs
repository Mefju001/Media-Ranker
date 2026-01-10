using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Movies.AddListOfMovies
{
    public record AddListOfGamesCommand(List<MovieRequest> requests) : IRequest<List<MovieResponse>>;
}
