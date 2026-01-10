using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Builder
{
    public class TvSeriesBuilder : ITvSeriesBuilder
    {
        private TvSeries tvSeries;
        public TvSeries Build()
        {
            if (tvSeries.genre == null)
                throw new InvalidOperationException("Tv series must have a Genre set.");
            return tvSeries;
        }

        public ITvSeriesBuilder CreateNew(string title, string description)
        {
            tvSeries = new TvSeries
            {
                title = title,
                description = description,
                Reviews = new List<Review>()
            };
            return this;
        }

        public ITvSeriesBuilder WithDefaultReview(Review review)
        {
            ArgumentNullException.ThrowIfNull(review);
            tvSeries.Reviews.Add(review);
            return this;
        }

        public ITvSeriesBuilder WithGenre(Genre Genre)
        {
            ArgumentNullException.ThrowIfNull(Genre);
            tvSeries.genre = Genre;
            return this;
        }

        public ITvSeriesBuilder WithMetadata(int? seasons, int? episodes, string? network, EStatus? status)
        {
            tvSeries.Seasons = seasons ?? 0;
            tvSeries.Episodes = episodes ?? 0;
            tvSeries.Network = network ?? string.Empty;
            tvSeries.Status = status ?? EStatus.Unknown;
            return this;
        }
    }
}
