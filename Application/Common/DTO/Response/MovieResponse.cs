namespace Application.Common.DTO.Response
{
    public record MovieResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        TimeSpan Duration,
        bool IsCinemaRelease

        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews), IResponse;

}
