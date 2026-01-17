using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesFilter
    {
        public IQueryable<TvSeriesDomain> Filter(IQueryable<TvSeriesDomain> query, Expression<Func<TvSeriesDomain, bool>> filterPredicate);
    }
}
