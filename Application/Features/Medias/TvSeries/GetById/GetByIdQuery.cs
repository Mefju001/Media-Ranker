using Application.Features.Common.Interfaces;
using Application.Features.Medias.TvSeries.Common;

namespace Application.Features.Medias.TvSeries.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<TvSeriesResponse?>;

}
