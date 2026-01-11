using Application.Features.GamesManagement.GetGamesByCriteria;
using Application.Features.GamesServices.GetGamesByCriteria;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Application.Features.Games.GetMovieById;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Specification;

namespace Infrastructure.Specification.BuildPredicate.Game
{
    public class GameBuildPredicate : IGameBuildPredicate
    {
        public Expression<Func<WebApplication1.Domain.Entities.Game, bool>> BuildPredicate(GetGamesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<WebApplication1.Domain.Entities.Game>();
            if (!string.IsNullOrEmpty(query.title))
            {
                finalPredicate = finalPredicate.And(m => m.title.Contains(query.title));
            }
            if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.genre.name.Contains(query.genreName));
            }
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
