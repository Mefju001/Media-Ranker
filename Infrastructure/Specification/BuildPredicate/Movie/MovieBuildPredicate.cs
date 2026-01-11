using Application.Features.MoviesManagement.GetMoviesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Application.Features.Movies.GetMoviesByCriteria;
using WebApplication1.Application.Features.TvSeries.GetMoviesByCriteria;
using WebApplication1.Infrastructure.Specification;

namespace Infrastructure.Specification.BuildPredicate.Movie
{
    public class MovieBuildPredicate:IMovieBuildPredicate
    {
        public Expression<Func<WebApplication1.Domain.Entities.Movie, bool>> BuildPredicate(GetMoviesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<WebApplication1.Domain.Entities.Movie>();
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
