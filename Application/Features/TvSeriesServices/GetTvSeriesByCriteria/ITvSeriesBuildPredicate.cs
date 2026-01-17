using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesBuildPredicate
    {
        Expression<Func<TvSeriesDomain, bool>> build(GetTvSeriesByCriteriaQuery query);
    }
}
