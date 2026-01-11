using Application.Features.GamesServices.GetGamesByCriteria;
using System.Linq.Expressions;
using WebApplication1.Domain.Entities;

namespace Application.Features.GamesManagement.GetGamesByCriteria
{
    public interface IGameBuildPredicate
    {
        public Expression<Func<Game, bool>> BuildPredicate(GetGamesByCriteriaQuery query);
    }
}
