using Domain.Enums;

namespace Application.Common.DTO.Response
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
