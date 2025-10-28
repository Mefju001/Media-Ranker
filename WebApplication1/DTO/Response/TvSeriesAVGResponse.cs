using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record TvSeriesAVGResponse
        (string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        int Seasons,
        int Episodes,
        string? Network,
        EStatus Status,
        double Avarage
        ) : MediaResponse(Title, Description, Genre, ReleaseDate, Language, Reviews)
    {
        public TvSeriesAVGResponse(TvSeriesResponse tvSeries, double avg) : this(tvSeries.Title,
            tvSeries.Description, tvSeries.Genre, tvSeries.ReleaseDate, tvSeries.Language,
            tvSeries.Reviews, tvSeries.Seasons, tvSeries.Episodes, tvSeries.Network,
            tvSeries.Status, avg)
        { }
    }
}
