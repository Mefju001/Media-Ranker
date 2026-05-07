using Application.Features.Common.Interfaces;

namespace Application.Features.Genres.GetAll
{
    public record GetAllQuery : IQuery<List<GenreResponse>>
    {
    }
}
