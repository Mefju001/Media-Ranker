using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Application.Features.TvSeries.GetMoviesByCriteria;
using WebApplication1.Domain.Entities;

namespace Application.Features.TvSeriesManagement.GetTvSeriesByCriteria
{
    public interface IBuildPredicate
    {
        Expression<Func<TvSeries, bool>> build(GetTvSeriesByCriteriaQuery query);
    }
}
