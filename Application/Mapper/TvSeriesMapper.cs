using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Features.TvSeries.TvSeriesUpsert;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
{
    public class TvSeriesMapper
    {
        public static TvSeriesResponse ToTvSeriesResponse(TvSeries tvSeries)
        {
            return new TvSeriesResponse(
                tvSeries.Id,
                tvSeries.title,
                tvSeries.description,
                GenreMapper.ToResponse(tvSeries.genre),
                tvSeries.ReleaseDate,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(tvSeries.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status);
        }
        public static void UpdateEntity(TvSeries tvSeries, UpsertTvSeriesCommand tvSeriesRequest, Genre genre)
        {
            tvSeries.title = tvSeriesRequest.title;
            tvSeries.description = tvSeriesRequest.description;
            tvSeries.genre = genre;
            tvSeries.ReleaseDate = tvSeriesRequest.ReleaseDate;
            tvSeries.Language = tvSeriesRequest.Language;
            tvSeries.Seasons = tvSeriesRequest.Seasons;
            tvSeries.Episodes = tvSeriesRequest.Episodes;
            tvSeries.Network = tvSeriesRequest.Network;
            tvSeries.Status = tvSeriesRequest.Status;
        }
    }
}
