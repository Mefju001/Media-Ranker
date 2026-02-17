using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public interface ITvSeriesSortAndFilterService
    {
        Task<IQueryable<TvSeries>> Handler(GetTvSeriesByCriteriaQuery request);
    }
}
