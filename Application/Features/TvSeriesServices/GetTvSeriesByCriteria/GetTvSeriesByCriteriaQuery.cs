using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Features.TvSeries.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaQuery() : IRequest<List<TvSeriesResponse>>
    {
        // PARAMETRY FILTROWANIA (Dla Wzorca Specyfikacja)

        public string? TitleSearch { get; set; }
        public double? MinRating { get; set; }
        public int? ReleaseYear { get; set; }
        public string? genreName { get; set; }
        public int? seasons { get; set; }
        public int? episodes { get; set; }
        public string? network { get; set; }
        public EStatus? status { get; set; }


        // PARAMETRY SORTOWANIA (Dla Wzorca Strategia)

        public string? SortByField { get; set; }
        public bool IsDescending { get; set; } = false;

    }

}
