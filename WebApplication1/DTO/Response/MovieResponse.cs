namespace WebApplication1.DTO.Response
{
    public record MovieResponse(
        string Title,
        string Description,
        GenreResponse Genre,
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        TimeSpan Duration,
        bool IsCinemaRelease
        ) :MediaResponse(Title, Description, Genre, ReleaseDate, Language, Reviews);
}
