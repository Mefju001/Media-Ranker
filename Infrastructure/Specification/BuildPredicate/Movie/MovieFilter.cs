using Application.Features.MoviesManagement.GetMoviesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Specification.BuildPredicate.Movie
{
    public class MovieFilter:IMovieFilter
    {
        public IQueryable<WebApplication1.Domain.Entities.Movie> Filter(IQueryable<WebApplication1.Domain.Entities.Movie> query, Expression<Func<WebApplication1.Domain.Entities.Movie, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
