using Domain.Entity;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesSortAndFilterService
    {
        Task<IQueryable<TvSeries>> Handler(GetTvSeriesByCriteriaQuery request);
    }
}
