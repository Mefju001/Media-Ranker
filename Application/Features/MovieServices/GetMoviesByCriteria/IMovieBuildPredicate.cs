using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieBuildPredicate
    {
        public Expression<Func<MovieDomain, bool>> BuildPredicate(GetMoviesByCriteriaQuery query);
    }
}
