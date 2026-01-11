using System.Linq.Expressions;
using WebApplication1.Application.Features.Movies.GetMoviesByCriteria;

namespace Application.Features.MoviesManagement.GetMoviesByCriteria
{
    public interface IMovieBuildPredicate
    {
        public Expression<Func<WebApplication1.Domain.Entities.Movie, bool>> BuildPredicate(GetMoviesByCriteriaQuery query);
    }
}
