using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class TvSeriesRepository : ITvSeriesRepository
    {
        private readonly AppDbContext appDbContext;
        public TvSeriesRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<TvSeriesDomain>> GetAll(CancellationToken cancellationToken)
        {
            return await appDbContext.TvSeries.ToListAsync(cancellationToken);
        }
        public async Task<TvSeriesDomain?> GetTvSeriesById(int id)
        {
            return await appDbContext.TvSeries.FirstOrDefaultAsync(tv => tv.Id == id);
        }
        public async Task Delete(TvSeriesDomain tvSeriesDomain)
        {
            appDbContext.TvSeries.Remove(tvSeriesDomain);
        }
        public async Task<TvSeriesDomain> AddTvSeriesAsync(TvSeriesDomain tvSeriesDomain)
        {
            var tvSeries = await appDbContext.TvSeries.AddAsync(tvSeriesDomain);
            return tvSeries.Entity;
        }
        public async Task<IQueryable<TvSeriesDomain>> AsQueryable()
        {
            return appDbContext.TvSeries
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
        }
        public async Task<List<TvSeriesDomain>> ToListAsync(IQueryable<TvSeriesDomain>query,CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }
        public async Task AddListOfTvSeries(List<TvSeriesDomain> list)
        {
            await appDbContext.TvSeries.AddRangeAsync(list);
        }
    }
}
