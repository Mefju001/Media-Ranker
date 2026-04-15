using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.MovieServices.AddListOfMovies
{
    public record AddListOfMoviesCommand(List<MovieRequest> movies) : ICommand<List<MovieResponse>>;
}
