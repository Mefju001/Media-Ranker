using Application.Common.DTO.Response;
using Application.Common.Interfaces;


namespace Application.Features.GenreServices
{
    public record GetGenresQuery : IQuery<List<GenreResponse>>
    {
    }
}
