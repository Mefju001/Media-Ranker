using Application.Features.Common.DTO.Response;
using Application.Features.Common.Interfaces;
using Application.Features.Genre.GetAll;
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
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews), IResponse;
}
