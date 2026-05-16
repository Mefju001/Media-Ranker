using Application.Features.Common.Interfaces;
using Application.Features.Genres.Common;
using Application.Features.TvSeries.Common;
using Domain.Enums;

namespace Application.Features.TvSeries.Upsert
{
    public record UpsertCommand(
        Guid? id,
        string title,
        string description,
        GenreRequest genre,
        DateTime ReleaseDate,
        string Language,
        int Seasons,
        int Episodes,
        string Network,
        EStatus Status) : ICommand<TvSeriesResponse>, ISendNotificationCommand;
}
