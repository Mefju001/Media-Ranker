using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameBuildPredicate
    {
        public Expression<Func<GameDomain, bool>> BuildPredicate(GetGamesByCriteriaQuery query);
    }
}
