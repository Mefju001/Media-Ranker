using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.MovieServices.GetMovieById
{
    public record GetMovieByIdQuery(int id) : IRequest<MovieResponse?>;

}
