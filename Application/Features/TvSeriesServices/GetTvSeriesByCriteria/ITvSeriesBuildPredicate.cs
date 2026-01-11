using System.Linq.Expressions;
using WebApplication1.Application.Features.TvSeries.GetTvSeriesByCriteria;
using WebApplication1.Domain.Entities;

namespace Application.Features.TvSeriesManagement.GetTvSeriesByCriteria
{
    public interface ITvSeriesBuildPredicate
    {
        Expression<Func<TvSeries, bool>> build(GetTvSeriesByCriteriaQuery query);
    }
}
