using Application.Features.GamesServices.GetGamesByCriteria;
using Domain.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Specification.BuildPredicate.Game
{
    public class GameBuildPredicate : IGameBuildPredicate
    {
        public Expression<Func<GameDomain, bool>> BuildPredicate(GetGamesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<GameDomain>();
            if (!string.IsNullOrEmpty(query.title))
            {
                finalPredicate = finalPredicate.And(m => m.Title.Contains(query.title));
            }
           /*if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.GenreId.name.Contains(query.genreName));
            }*/
            if (query.MinRating.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.Stats!.AverageRating >= query.MinRating.Value);
            }
            if (query.releaseDate.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.ReleaseDate.Year == query.releaseDate);
            }
            return finalPredicate;
        }
    }
}
