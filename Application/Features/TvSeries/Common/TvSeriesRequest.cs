using Application.Features.Genres.Common;
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
        ETvSeriesStatus Status
        )
    {
    }
}
