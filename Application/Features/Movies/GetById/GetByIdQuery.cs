using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.GetMovieById
{
    public record GetByIdQuery(Guid id) : IQuery<MovieResponse?>;

}
