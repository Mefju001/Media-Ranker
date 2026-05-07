using Application.Features.Common.Interfaces;
using Application.Features.Movie.Common;

namespace Application.Features.Movies.AddRange
{
    public record AddRangeCommand(List<MovieRequest> movies) : ICommand<List<MovieResponse>>;
}
