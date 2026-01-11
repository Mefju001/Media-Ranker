using Application.Features.GamesManagement.GetGamesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Domain.Entities;

namespace Infrastructure.Specification.BuildPredicate.Game
{
    public class GameFilter:IGameFilter
    {
        public IQueryable<WebApplication1.Domain.Entities.Game> Filter(IQueryable<WebApplication1.Domain.Entities.Game> query, Expression<Func<WebApplication1.Domain.Entities.Game, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
