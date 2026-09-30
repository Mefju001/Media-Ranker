using Application.Features.Common.Interfaces;

namespace Application.Features.Genres.GetGenres
{
    public record GetGenresQuery(EMediaType? Media) : IQuery<List<GenreResponse>>;
}
