using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Domain.Entities;

namespace Application.Features.MoviesManagement.GetMoviesByCriteria
{
    public interface IGameFilter
    {
        public IQueryable<Game> Filter(IQueryable<Game> query, Expression<Func<Game, bool>> filterPredicate);
    }
}
