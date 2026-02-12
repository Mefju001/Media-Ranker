using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ITvSeriesRepository
    {
        Task<TvSeriesDomain?> GetTvSeriesById(int id);
        Task<List<TvSeriesDomain>> GetAll(CancellationToken cancellationToken);
        Task Delete(TvSeriesDomain tvSeriesDomain);
        Task AddListOfTvSeries(List<TvSeriesDomain> list);
        Task<TvSeriesDomain> AddTvSeriesAsync(TvSeriesDomain tvSeriesDomain);
        Task<IQueryable<TvSeriesDomain>> AsQueryable();
        Task<List<TvSeriesDomain>> ToListAsync(IQueryable<TvSeriesDomain> query, CancellationToken cancellationToken);
    }
}
