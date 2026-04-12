using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesSortAndFilterService
    {
        Task<List<TvSeriesResponse>> Handler(GetTvSeriesByCriteriaQuery request, CancellationToken ct);
    }
}
