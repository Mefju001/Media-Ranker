using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.MovieServices.AddListOfMovies
{
    public record AddListOfMoviesCommand(List<MovieRequest> requests) : IRequest<List<MovieResponse>>;
}
