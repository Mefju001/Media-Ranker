using System.Linq.Expressions;

namespace Application.Features.MoviesManagement.GetMoviesByCriteria
{
    public interface IMovieFilter
    {
        public IQueryable<WebApplication1.Domain.Entities.Movie> Filter(IQueryable<WebApplication1.Domain.Entities.Movie> query, Expression<Func<WebApplication1.Domain.Entities.Movie, bool>> filterPredicate);
    }
}
