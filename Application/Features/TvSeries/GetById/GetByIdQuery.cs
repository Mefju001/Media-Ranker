using Application.Features.Common.Interfaces;
using Application.Features.TvSeries.Common;

namespace Application.Features.TvSeries.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<TvSeriesResponse?>;

}
