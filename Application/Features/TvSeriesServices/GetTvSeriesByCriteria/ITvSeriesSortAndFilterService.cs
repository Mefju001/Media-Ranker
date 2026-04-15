using Application.Common.DTO.Response;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesSortAndFilterService
    {
        Task<List<TvSeriesResponse>> Handler(GetTvSeriesByCriteriaQuery request, CancellationToken ct);
    }
}
