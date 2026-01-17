using Application.Features.MovieServices.GetMoviesByCriteria;
using Domain.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Specification.BuildPredicate.Movie
{
    public class MovieBuildPredicate : IMovieBuildPredicate
    {
        public Expression<Func<MovieDomain, bool>> BuildPredicate(GetMoviesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<MovieDomain>();
            if (!string.IsNullOrEmpty(query.TitleSearch))
            {
                finalPredicate = finalPredicate.And(m => m.Title.Contains(query.TitleSearch));
            }
            /*if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.genre.name.Contains(query.genreName));
            }*/
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
