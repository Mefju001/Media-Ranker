using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.MovieServices.GetAll
{
    public record GetAllQuery : IRequest<List<MovieResponse>>;
}
