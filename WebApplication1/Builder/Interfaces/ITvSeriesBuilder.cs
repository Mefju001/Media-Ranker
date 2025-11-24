using WebApplication1.Models;

namespace WebApplication1.Builder.Interfaces
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
