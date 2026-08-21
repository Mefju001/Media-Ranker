using Application.Features.Genres.Common;
using Domain.Enums;

namespace Application.Features.Medias.TvSeries.Common
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
        string Status
        )
    {
    }
}
