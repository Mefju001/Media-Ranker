using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.GetMovieById
{
    public record GetMovieByIdQuery(int id) : IQuery<MovieResponse?>;

}
