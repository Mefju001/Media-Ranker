using WebApplication1.Domain.Entities;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Interfaces
{
    public interface ITvSeriesBuilder
    {
        ITvSeriesBuilder CreateNew(string title, string description);
        ITvSeriesBuilder WithMetadata(
            int? seasons,
            int? episodes,
            string? network,
            EStatus? status);
        ITvSeriesBuilder WithGenre(Genre Genre);
        ITvSeriesBuilder WithDefaultReview(Review review);
        TvSeries Build();
    }
}
