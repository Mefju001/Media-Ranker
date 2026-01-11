using Application.Features.TvSeriesManagement.GetTvSeriesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Application.Features.TvSeries.GetMoviesByCriteria;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Specification;
namespace Infrastructure.Specification.BuildPredicate.TvSeries
{
    public class TvSeriesBuildPredicate:ITvSeriesBuildPredicate
    {
        public Expression<Func<WebApplication1.Domain.Entities.TvSeries, bool>> build(GetTvSeriesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<WebApplication1.Domain.Entities.TvSeries>();
            if (!string.IsNullOrEmpty(query.TitleSearch))
            {
                finalPredicate = finalPredicate.And(m => m.title.Contains(query.TitleSearch));
            }
            if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.genre.name.Contains(query.genreName));
            }
            if (query.MinRating.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.Stats!.AverageRating >= query.MinRating.Value);
            }
            if (query.ReleaseYear.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.ReleaseDate.Year == query.ReleaseYear);
            }
            return finalPredicate;
        }
    }
}
