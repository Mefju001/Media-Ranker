using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebApplication1.Application.Features.Games.GetMovieById;
using WebApplication1.Application.Features.Games.GetMoviesByCriteria;
using WebApplication1.Domain.Entities;

namespace Application.Features.MoviesManagement.GetMoviesByCriteria
{
    public interface IGameBuildPredicate
    {
        public Expression<Func<Game, bool>> BuildPredicate(GetGamesByCriteriaQuery query);
    }
}
