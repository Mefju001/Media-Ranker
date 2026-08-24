using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Movies.Common;

namespace Application.Features.UserInteractions.Rankings.Get
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
