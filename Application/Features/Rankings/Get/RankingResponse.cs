using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;

namespace Application.Features.Rankings.Get
{
    public record RankingResponse(
        Guid Id,
        string Title,
        string Description,
        DateTime ReleaseDate,
        string MediaType,
        GenreResponse Genre,
        MediaStatsResponse Stats
    );
}
