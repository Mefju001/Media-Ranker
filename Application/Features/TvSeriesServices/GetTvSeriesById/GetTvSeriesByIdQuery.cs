using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public record GetTvSeriesByIdQuery(Guid id) : IQuery<TvSeriesResponse?>;

}
