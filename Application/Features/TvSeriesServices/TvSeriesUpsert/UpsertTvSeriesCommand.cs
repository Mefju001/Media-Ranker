using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Features.TvSeries.TvSeriesUpsert
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
        EStatus Status) : IRequest<TvSeriesResponse>;
}
