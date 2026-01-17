using Application.Features.GamesServices.GetGamesByCriteria;
using Domain.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Specification.BuildPredicate.Game
{
    public class GameFilter : IGameFilter
    {
        public IQueryable<GameDomain> Filter(IQueryable<GameDomain> query, Expression<Func<GameDomain, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
