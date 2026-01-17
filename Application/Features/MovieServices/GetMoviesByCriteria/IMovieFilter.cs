using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieFilter
    {
        public IQueryable<MovieDomain> Filter(IQueryable<MovieDomain> query, Expression<Func<MovieDomain, bool>> filterPredicate);
    }
}
