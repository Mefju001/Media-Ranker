namespace WebApplication1.DTO.Response
{
    public record GameResponse(
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        string? Developer,
        string Platform
        ) : MediaResponse(Title, Description, Genre, ReleaseDate, Language, Reviews);
}
