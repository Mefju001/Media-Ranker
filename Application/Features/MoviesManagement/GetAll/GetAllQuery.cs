using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Movies.GetAll
{
    public record GetAllQuery : IRequest<List<MovieResponse>>;
}
