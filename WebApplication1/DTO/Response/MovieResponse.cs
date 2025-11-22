namespace WebApplication1.DTO.Response
{
    public record MovieResponse(
        int id,
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
        ) : MediaResponse(Title, Description, Genre, ReleaseDate, Language, Reviews);
}
