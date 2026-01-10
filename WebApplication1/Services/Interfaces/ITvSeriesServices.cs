using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Features.TvSeries.GetTvSeriesByCriteria;

namespace WebApplication1.Services.Interfaces
{
    public interface ITvSeriesServices
    {
        Task<List<TvSeriesResponse>> GetAllAsync();
        Task<List<TvSeriesResponse>> GetMoviesByCriteriaAsync(TvSeriesQuery tvSeriesQuery);
        Task<TvSeriesResponse?> GetById(int id);
        Task<bool> Delete(int id);
        Task<TvSeriesResponse> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest);

    }
}
