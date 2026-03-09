using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ITvSeriesRepository
    {
        Task<TvSeries?> GetTvSeriesById(int id);
        Task<List<TvSeries>> GetAll(CancellationToken cancellationToken);
        Task Delete(TvSeries tvSeriesDomain);
        Task AddListOfTvSeries(List<TvSeries> list);
        Task<TvSeries> AddTvSeriesAsync(TvSeries tvSeriesDomain);
        IQueryable<TvSeries> AsQueryable();
        Task<List<TvSeries>> ToListAsync(IQueryable<TvSeries> query, CancellationToken cancellationToken);
    }
}
