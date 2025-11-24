using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record GameResponse(
        int id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        string? Developer,
        EPlatform Platform
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews);
}
