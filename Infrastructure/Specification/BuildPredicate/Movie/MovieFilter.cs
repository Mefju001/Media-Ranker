using Application.Features.MovieServices.GetMoviesByCriteria;
using Domain.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Specification.BuildPredicate.Movie
{
    public class MovieFilter : IMovieFilter
    {
        public IQueryable<MovieDomain> Filter(IQueryable<MovieDomain> query, Expression<Func<MovieDomain, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
