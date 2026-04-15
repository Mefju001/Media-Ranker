using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.MovieServices.GetMovieById
{
    public record GetMovieByIdQuery(Guid id) : IQuery<MovieResponse?>;

}
