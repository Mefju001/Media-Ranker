using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.GetAll
{
    public record GetAllQuery : IQuery<List<MovieResponse>>;
}
