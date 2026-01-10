using Application.Features.MoviesManagement.GetMoviesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Domain.Entities;

namespace Infrastructure.Specification.BuildPredicate.Game
{
    internal class GameFilter:IGameFilter
    {
        public IQueryable<Game> Filter(IQueryable<Game> query, Expression<Func<Game, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
