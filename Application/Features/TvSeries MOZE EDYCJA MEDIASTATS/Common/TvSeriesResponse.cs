using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;
using Application.Features.Reviews.Common;
using Domain.Enums;

namespace Application.Features.TvSeries.Common
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
        EStatus Status
        ): IResponse;
}
