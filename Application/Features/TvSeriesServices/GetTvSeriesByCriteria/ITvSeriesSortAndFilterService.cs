using Domain.Aggregate;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesSortAndFilterService
    {
        IQueryable<TvSeries> Handler(GetTvSeriesByCriteriaQuery request);
    }
}
