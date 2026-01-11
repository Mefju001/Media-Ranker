using System.Linq.Expressions;
using WebApplication1.Domain.Entities;

namespace Application.Features.GamesManagement.GetGamesByCriteria
{
    public interface IGameFilter
    {
        public IQueryable<Game> Filter(IQueryable<Game> query, Expression<Func<Game, bool>> filterPredicate);
    }
}
