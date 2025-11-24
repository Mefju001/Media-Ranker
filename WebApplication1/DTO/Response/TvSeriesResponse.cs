using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record TvSeriesResponse(
        int id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        int Seasons,
        int Episodes,
        string? Network,
        EStatus Status
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews);
}
