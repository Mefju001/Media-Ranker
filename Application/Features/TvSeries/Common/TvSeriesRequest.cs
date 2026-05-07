using Application.Features.Common.DTO.Request;
using Domain.Enums;

namespace Application.Features.TvSeries.Common
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
