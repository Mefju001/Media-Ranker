using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameFilter
    {
        public IQueryable<GameDomain> Filter(IQueryable<GameDomain> query, Expression<Func<GameDomain, bool>> filterPredicate);
    }
}
