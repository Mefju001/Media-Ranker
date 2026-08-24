using Application.Features.Common.Interfaces;
using Application.Features.Common.Models;
using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Movies.Common;
using Application.Features.UserInteractions.Reviews.Common;

namespace Application.Features.Medias.TvSeries.Common
{
    public record TvSeriesResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        int Seasons,
        int Episodes,
        string? Network,
        string Status
        ): MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, MediaStats), IResponse;
}
