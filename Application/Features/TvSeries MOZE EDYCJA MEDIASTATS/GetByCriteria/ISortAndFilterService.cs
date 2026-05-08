using Application.Features.TvSeries.Common;

namespace Application.Features.TvSeries.GetByCriteria
{
    public interface ISortAndFilterService
    {
        Task<List<TvSeriesResponse>> Handler(GetByCriteriaQuery request, CancellationToken ct);
    }
}
