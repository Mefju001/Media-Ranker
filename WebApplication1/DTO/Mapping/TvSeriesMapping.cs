using System.IO;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class TvSeriesMapping
    {
        public static TvSeriesResponse ToTvSeriesResponse(TvSeries tvSeries)
        {
            return new TvSeriesResponse(
                tvSeries.title,
                tvSeries.description,
                GenreMapping.ToResponse(tvSeries.genre),
                tvSeries.ReleaseDate,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status);
        }
        public static TvSeriesAVGResponse ToTvSeriesAVGResponse(TvSeries tvSeries, double avg)
        {
            var tvSeriesResponse = ToTvSeriesResponse(tvSeries);
            return new TvSeriesAVGResponse(tvSeriesResponse, avg);
        }
        public static void UpdateEntity(TvSeries tvSeries,TvSeriesRequest tvSeriesRequest,Genre genre)
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
