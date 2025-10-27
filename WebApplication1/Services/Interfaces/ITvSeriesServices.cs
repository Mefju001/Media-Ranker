using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface ITvSeriesServices
    {
        Task<List<TvSeriesResponse>> GetAllAsync();
        Task<List<TvSeriesResponse>> GetSortAll(string sortDirection, string sortByfield);
        Task<List<TvSeriesResponse>> GetTvSeriesByAvrRating();
        Task<List<TvSeriesResponse>> GetTvSeries(string? name, string? genreName, string? directorName);
        Task<TvSeriesResponse> GetById(int id);
        Task<bool> Delete(int id);
        Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest);

    }
}
