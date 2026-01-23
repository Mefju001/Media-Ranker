using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITvSeriesRepository
    {
        Task<TvSeriesDomain?>GetTvSeriesById(int id);
        Task<List<TvSeriesDomain>> GetAll(CancellationToken cancellationToken);
        Task Delete(TvSeriesDomain tvSeriesDomain);
        Task AddListOfTvSeries(List<TvSeriesDomain> list);
        Task<TvSeriesDomain>AddTvSeriesAsync(TvSeriesDomain tvSeriesDomain);
        Task<IQueryable<TvSeriesDomain>> AsQueryable();
    }
}
