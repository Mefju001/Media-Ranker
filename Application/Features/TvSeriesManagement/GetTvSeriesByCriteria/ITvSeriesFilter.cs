using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Features.TvSeriesManagement.GetTvSeriesByCriteria
{
    public interface ITvSeriesFilter
    {
        public IQueryable<WebApplication1.Domain.Entities.TvSeries> Filter(IQueryable<WebApplication1.Domain.Entities.TvSeries> query, Expression<Func<WebApplication1.Domain.Entities.TvSeries, bool>> filterPredicate);
    }
}
