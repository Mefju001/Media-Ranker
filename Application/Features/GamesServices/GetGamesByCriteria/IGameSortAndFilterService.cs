using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        Task<IQueryable<GameDomain>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request);
    }
}
