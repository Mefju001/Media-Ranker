using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Features.TvSeriesServices.TvSeriesUpsert
{
    public record UpsertTvSeriesCommand(
        int? id,
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
