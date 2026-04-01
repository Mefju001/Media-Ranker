using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;


namespace Application.Features.GenreServices
{
    public record GetGenresQuery : IQuery<List<GenreResponse>>
    {
    }
}
