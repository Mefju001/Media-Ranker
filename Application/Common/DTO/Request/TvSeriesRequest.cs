using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Common.DTO.Request
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
