using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ITvSeriesRepository
    {
        Task<TvSeries?> GetTvSeriesById(int id, CancellationToken cancellationToken);
        Task<List<TvSeries>> GetAll(CancellationToken cancellationToken);
        void Delete(TvSeries tv);
        Task AddListOfTvSeries(List<TvSeries> list, CancellationToken cancellationToken);
        Task<TvSeries> AddTvSeriesAsync(TvSeries tvSeriesDomain, CancellationToken cancellationToken);
        IQueryable<TvSeries> AsQueryable();
        Task<List<TvSeries>> ToListAsync(IQueryable<TvSeries> query, CancellationToken cancellationToken);
    }
}
