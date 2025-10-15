using WebApplication1.Models;

namespace WebApplication1.DTO.Request
{
    public record TvSeriesRequest(
        string title,
        string description,
        GenreRequest genre,
        DateTime ReleaseDate,
        string Language,
        int Seasons,
        int Episodes,
        string Network,
        EStatus Status
        )
    {
    }
}
