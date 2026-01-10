using Application.Features.TvSeriesManagement.GetTvSeriesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Specification.BuildPredicate.TvSeries
{
    internal class TvSeriesFilter:ITvSeriesFilter
    {
        public IQueryable<WebApplication1.Domain.Entities.TvSeries> Filter(IQueryable<WebApplication1.Domain.Entities.TvSeries> query, Expression<Func<WebApplication1.Domain.Entities.TvSeries, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
