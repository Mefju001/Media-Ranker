using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Enums;

namespace Application.Features.TvSeriesServices.TvSeriesUpsert
{
    public record UpsertTvSeriesCommand(
        Guid? id,
        string title,
        string description,
        GenreRequest genre,
        DateTime ReleaseDate,
        string Language,
        int Seasons,
        int Episodes,
        string Network,
        EStatus Status) : ICommand<TvSeriesResponse>;
}
