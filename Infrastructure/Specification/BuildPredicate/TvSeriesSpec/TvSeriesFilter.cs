using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Specification.BuildPredicate.TvSeriesSpec
{
    public class TvSeriesFilter : ITvSeriesFilter
    {
        public IQueryable<TvSeriesDomain> Filter(IQueryable<TvSeriesDomain> query, Expression<Func<TvSeriesDomain, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
