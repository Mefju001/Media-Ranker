namespace WebApplication1.DTO.Response
{
    public record TvSeriesResponse(
        string Title,
        string Description,
        GenreResponse Genre,
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        int Seasons,
        int Episodes,
        string? Network,
        string? Status
        ) :MediaResponse(Title, Description, Genre, Director, ReleaseDate, Language, Reviews);
}
