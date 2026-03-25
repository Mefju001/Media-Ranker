using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class TvSeriesRepository : ITvSeriesRepository
    {
        private readonly AppDbContext appDbContext;
        public TvSeriesRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<TvSeries>> GetAll(CancellationToken cancellationToken)
        {
            return await appDbContext.TvSeries.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<TvSeries?> GetTvSeriesById(int id, CancellationToken cancellationToken)
        {
            return await appDbContext.TvSeries.AsNoTracking().FirstOrDefaultAsync(tv => tv.Id == id, cancellationToken);
        }
        public void Delete(TvSeries tv)
        {
            appDbContext.TvSeries.Remove(tv);
        }
        public async Task<TvSeries> AddTvSeriesAsync(TvSeries tvSeriesDomain, CancellationToken cancellationToken)
        {
            var tvSeries = await appDbContext.TvSeries.AddAsync(tvSeriesDomain, cancellationToken);
            return tvSeries.Entity;
        }
        public IQueryable<TvSeries> AsQueryable()
        {
            return appDbContext.TvSeries
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
        }
        public async Task<List<TvSeries>> ToListAsync(IQueryable<TvSeries> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task AddListOfTvSeries(List<TvSeries> list, CancellationToken cancellationToken)
        {
            await appDbContext.TvSeries.AddRangeAsync(list, cancellationToken);
        }
    }
}
