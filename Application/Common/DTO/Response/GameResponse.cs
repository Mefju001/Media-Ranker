using Domain.Enums;

namespace Application.Common.DTO.Response
{
    public record GameResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        string? Developer,
        List<EPlatform> Platforms
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews), IResponse;
}
