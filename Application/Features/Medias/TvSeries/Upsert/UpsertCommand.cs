using Application.Features.Common.Interfaces;
using Application.Features.Genres.Common;
using Application.Features.Medias.TvSeries.Common;
using Domain.Enums;

namespace Application.Features.Medias.TvSeries.Upsert
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
        ETvSeriesStatus Status) : ICommand<TvSeriesResponse>, ISendNotificationCommand;
}
