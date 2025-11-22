using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.QueryHandler.Query;

namespace WebApplication1.Services.Interfaces
{
    public interface ITvSeriesServices
    {
        Task<List<TvSeriesResponse>> GetAllAsync();
        Task<List<TvSeriesResponse>> GetMoviesByCriteriaAsync(TvSeriesQuery tvSeriesQuery);
        Task<TvSeriesResponse> GetById(int id);
        Task<bool> Delete(int id);
        Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest);

    }
}
